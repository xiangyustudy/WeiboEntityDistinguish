using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Text.RegularExpressions;

namespace WeiboEntityDistinguish
{
    class HttpUtil
    {
         public static string GetHtmlText(string url)
        {
            WebClient wc = new WebClient();
            wc.Encoding = Encoding.UTF8;
            string html = wc.DownloadString(url);

            return html;
        }
         
        public static string GetMultiMeaningListURL(string first_page_html)
          {
              int start = first_page_html.IndexOf("href=\"/view/");
              int end = first_page_html.IndexOf("个义项");


              string row_html = first_page_html.Substring(start, end - start);

              List<string> all_link = new List<string>();
              foreach (Match m in Regex.Matches(row_html, "/view/(.*)=1", RegexOptions.Multiline))
              {
                  string all_link_url = m.Groups[1].Value;

                  //===============临时输出==================
                  Console.WriteLine(all_link_url);
                  all_link.Add(all_link_url);
              }

              return "http://baike.baidu.com/view/" + all_link[0]+"=1";

          }

        //获取百度百科的描述
        private static string GetBaikeDescription(string first_page_html) 
	    {

            Match m = Regex.Match(first_page_html, "<meta(?<other1>.*?)Description(?<other2>.*?)content=\"(?<description>.*?)>", RegexOptions.IgnoreCase);
            string description = m.Groups["description"].Value;
	        return description;
	    		
	    }
        
        //获取百度百科标签组
        public static string GetBaikeLabs(string item_html)
	    {
            //===============临时输出==================
            Console.WriteLine(item_html.IndexOf("词条标签："));
		    if(item_html.IndexOf("词条标签：")>=0)
		    {

                Match m = Regex.Match(item_html, "<div (?<other1>.*?)词条标签：(?<target>.*?)open-tag-collapse(?<other2>.*?)</div>", RegexOptions.IgnoreCase);
                string target_str = m.Groups["target"].Value;
              
                //===============临时输出==================
                Console.WriteLine(target_str);

                
			    List<String> lab_list=new List<String>();
                foreach (Match mm in Regex.Matches(target_str, "<a href=\"(?<other>.*?)\">(?<lab>.*?)</a>", RegexOptions.Multiline))
                {
                    string lab = mm.Groups["lab"].Value;

                    //===============临时输出==================
                    Console.WriteLine(lab);
                    lab_list.Add(lab);
                }

                foreach (Match mm in Regex.Matches(target_str, "<a class=\"(?<other>.*?)\">(?<lab>.*?)</a>", RegexOptions.Multiline))
                {
                    string lab = mm.Groups["lab"].Value;

                    //===============临时输出==================
                    Console.WriteLine(lab);
                    lab_list.Add(lab);
                }
                //===============临时输出==================
                Console.WriteLine(lab_list.Count);

			   
			    return lab_list.Count>0?string.Join(",",lab_list.ToArray()):"NIL";

		    }
		    else
			    return "NIL";

	    }
	
        //获取百度查询数目
        private static int GetItemQuery(string explanation) {
		
	        explanation=explanation.Replace(" ", "");
		    string query_url="http://www.baidu.com/s?ie=utf-8&f=8&rsv_bp=1&tn=baidu&wd="+explanation;
		    string query_html=GetHtmlText(query_url);

            Match m = Regex.Match(query_html, "<div (?<other1>.*?)nums(?<nums>.*?)</div>", RegexOptions.IgnoreCase);
            string nums_str = m.Groups["nums"].Value;
		
		    //=========临时输出==========
		    Console.WriteLine(explanation+" "+nums_str);
            String num = nums_str.Substring(nums_str.IndexOf("约") + 1, nums_str.IndexOf("个") - nums_str.IndexOf("约")-1);

            return System.Int32.Parse(num.Replace(",", "")); 
	}

         //获取百科页面的title
        public static string GetBaikeTitle(string baikehtml)
		{
			if(baikehtml.IndexOf("X-UA-Compatible")>0)
			{
				return "NIL";		
			}
			else
			{
				int beginIndex2=baikehtml.IndexOf("<title>");
				int endIndex2=baikehtml.IndexOf("_百度百科</title>");	
				
				if(baikehtml.IndexOf("imgzd")>0)
					Console.WriteLine("百度拦截");
				
//				System.out.println(baikehtml); //---临时输出---
                string title=baikehtml.Substring(beginIndex2+7,endIndex2-beginIndex2-7);	
				
				return title;
			}
		}
		


        public static List<BaikeItem> GetMatchBaikeItem(string word,string weibo)
        {

            List<BaikeItem> match_baikeitem_list = new List<BaikeItem>();

            String url = "http://baike.baidu.com/search?word=" + word + "&pn=0&rn=10&enc=utf8";

            //===============临时输出==================
            Console.WriteLine(url);

            String search_html = GetHtmlText(url);  //获取搜索页面html

            //===============临时输出==================
		    if(search_html.IndexOf("imgzd")>0)
                Console.WriteLine("百度拦截");

            List<string> all_link = new List<string>();
            Console.WriteLine("匹配得到"); 
            foreach (Match m in Regex.Matches(search_html, "http://baike.baidu.com(.*?)htm", RegexOptions.Multiline))
            {
                string tmp_str = m.Groups[1].Value;
                tmp_str = "http://baike.baidu.com" + tmp_str + "htm";
                Console.WriteLine(tmp_str);
                all_link.Add(tmp_str);
            }
            Console.WriteLine("匹配完成"+all_link.Count); 

            if (all_link.Count == 0)  //如果百度百科完全搜不到，则返回原词
            {
                Console.WriteLine("百度百科尚未收录此词语");
                return match_baikeitem_list;
            }
            else
            {
                string first_link = all_link[0]; //获取搜索后第一个链接
                string first_page_html = GetHtmlText(first_link); //获取进入链接后的页面
			
            
                if(first_page_html.IndexOf("多义词")>0&&first_page_html.IndexOf("个义项")>0)
			    {
				    //获取list页面各个义项的url
				    string multi_meaning_list_url=GetMultiMeaningListURL(first_page_html);  //获取多义表页面URL
				    //==============临时输出===============
				    Console.WriteLine(multi_meaning_list_url);
				
				    //获取list页面html
				    string multi_meaning_list_html=GetHtmlText(multi_meaning_list_url);
				    string preprocess=GetBaikeTitle(multi_meaning_list_html);
				
                    //取得各个义项title、description、url、query
                     Console.WriteLine("匹配得到");
                      foreach (Match m in Regex.Matches(multi_meaning_list_html, "/subview/(?<url>.*?)htm(.*?)" + preprocess + "(?<des>.*?)</a>", RegexOptions.Multiline))
                     {
                        string a_row = m.ToString();

                        Console.WriteLine(a_row);

                        if (a_row.Length <= 150)
                        {
                            string explanation = preprocess + ":" + m.Groups["des"].Value.Substring(1);

                            string item_url = "http://baike.baidu.com/subview/" + m.Groups["url"].Value + "htm";

                            string item_html = GetHtmlText(item_url);
                            string title = GetBaikeTitle(item_html);  //获取义项页面的titile

                            string description = GetBaikeDescription(item_html);
                            string labs = GetBaikeLabs(item_html);
                            int query = GetItemQuery(explanation);

                
                            BaikeItem baike_item = new BaikeItem(preprocess, explanation, title, item_url, description, labs, query);
                            Console.WriteLine(baike_item.toString());

                            match_baikeitem_list.Add(baike_item);
                       

                        }

                    }


                      int size = match_baikeitem_list.Count;

                      //归一化查询次数
                      int[] query_arrs = new int[size];
                      for (int i = 0; i < size; i++)
                          query_arrs[i] = match_baikeitem_list[i].getQuery();
                      double[] normalization = CommonUtil.getNormalization(query_arrs);

                      //归一化微博名词组与词条标签、描述名词组的JD相似度 
                      List<String> weibo_list = StringUtil.GetNouns(NlpirUtil.GetNLPParseResult(weibo));
                      double[] jds = new double[size];
                      for (int i = 0; i < size; i++)
                      {
                          BaikeItem bi = match_baikeitem_list[i];

                          String labs = bi.getLabs();
                          if ("NIL".Equals(labs))
                          {
                              List<String> description_nous = StringUtil.GetNouns(NlpirUtil.GetNLPParseResult(bi.getDescription()));
                              jds[i] = CommonUtil.getJaccardDistance(description_nous, weibo_list);
                          }
                          else
                          {

                              List<String> labs_list = new List<String>(labs.Split(','));


                              double jd1 = CommonUtil.getJaccardDistance(labs_list, weibo_list);

                              List<String> description_nous = StringUtil.GetNouns(NlpirUtil.GetNLPParseResult(bi.getDescription()));
                              double jd2 = CommonUtil.getJaccardDistance(description_nous, weibo_list);

                              double aa = 0.3;
                              double bb = 0.7;
                              jds[i] = aa * jd1 + bb * jd2;

                          }

                      }


                      double a = 0.3;
                      double b = 0.7;
                      for (int i = 0; i < size; i++)
                      {
                          BaikeItem bi = match_baikeitem_list[i];
                          Console.WriteLine(normalization[i] + "  " + jds[i]);
                          double match = a * normalization[i] + b * jds[i];
                          Console.WriteLine(match);
                          bi.setMatch(match);
                      }

                      match_baikeitem_list.Sort(
                          (Comparison<BaikeItem>)delegate(BaikeItem bi1, BaikeItem bi2)
                         {
                             return bi2.getMatch() > bi1.getMatch() ? 1 : -1;
                         });

                }
                else
                {
                    String item_url = first_link;

                    String title = GetBaikeTitle(first_page_html).Trim();  //获取义项页面的titile
                    String description = GetBaikeDescription(first_page_html);
                    String labs = GetBaikeLabs(first_page_html);

                    BaikeItem bi = new BaikeItem();
                    bi.setUrl(item_url);
                    bi.setTitle(title);
                    bi.setDescription(description);

                    List<string> weibo_list = StringUtil.GetNouns(NlpirUtil.GetNLPParseResult(weibo));
                    List<string> labs_list = new List<string>(labs.Split(','));


                    double jd1 = CommonUtil.getJaccardDistance(labs_list, weibo_list);

                    List<string> description_nous = StringUtil.GetNouns(NlpirUtil.GetNLPParseResult(description));
                    double jd2 = CommonUtil.getJaccardDistance(description_nous, weibo_list);

                    double aa = 0.3;
                    double bb = 0.7;
                    double jd = aa * jd1 + bb * jd2;

                    Console.WriteLine(jd);
                    //if (jd >= 0.01)
                        match_baikeitem_list.Add(bi);
                }
            
            
            }

            return match_baikeitem_list;
   
        }
    }
}
