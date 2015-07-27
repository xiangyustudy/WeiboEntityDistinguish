using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NSoup;
using NSoup.Nodes;
using NSoup.Select;

namespace WeiboEntityDistinguish
{
    class SpiderUtil
    {
        public static String GetBaikeHtml(String url_path)
        {
            try
            {
                WebClient wc = new WebClient();
                wc.Encoding = Encoding.UTF8;
                string html = wc.DownloadString(url_path);
                return html;
            }
            catch (WebException ex)
            {
                System.Windows.MessageBox.Show("网络错误，请检查网络连接。", "错误");
            }

            return "";
        }

        public static String GetBaikeTitle(String html)
        {
            Document doc = NSoupClient.Parse(html, "UTF-8");

            String title = doc.Title;

            title = title.Replace("_百度百科", "");
            if (title.IndexOf("（") > 0)
                title = title.Substring(0, title.IndexOf("（"));

            return title;
        }

        public static String GetBaikeExplatationTitle(String html)
        {
            Document doc = NSoupClient.Parse(html, "UTF-8");

            String title = doc.Title;

            title = title.Replace("_百度百科", "");

            return title;
        }


        public static String GetBaikeMainContent(String html)
        {
            Document doc = NSoupClient.Parse(html, "UTF-8");

            Elements metas1 = doc.Select("div.para");

            StringBuilder sb = new StringBuilder();
            foreach (Element e1 in metas1)
            {
                String content = e1.Text();
                sb.Append(" " + content);
            }

            Elements metas2 = doc.Select("span.biTitle");
            foreach (Element e2 in metas2)
            {
                String content = e2.Text();
                content = content.Replace(' ', ' ');
                sb.Append(" " + content);
            }

            Elements metas3 = doc.Select(".open-tag");
            foreach (Element e3 in metas3)
            {
                String content = e3.Text();
                sb.Append(" " + content);
            }

            Elements metas4 = doc.Select(".taglist");
            foreach (Element e4 in metas4)
            {
                String content = e4.Text();
                sb.Append(" " + content);
            }

            return sb.ToString();

        }

        public static String GetDescription(String html)
        { 
            	Document doc=NSoupClient.Parse(html,"UTF-8");
		        Elements metas=doc.Head.Select("meta");
		
		        String description=null;
		

		        foreach(Element meta in metas) 
		        {  
	                 String content = meta.Attr("content");

                     if ("description".Equals(meta.Attr("name"), StringComparison.OrdinalIgnoreCase))
	                        description=content;   
	            }

                if (description == null)
                {
                    throw new Exception("xxxx");
                }
	  
	            return description;

        }

        public static List<String> GetTermsURL(String word)
        {
            List<String> terms_url_list = new List<String>();

            String url = "http://baike.baidu.com/search?word=" + word + "&pn=0&rn=10&enc=utf8";
            //===============临时输出==================
            Console.WriteLine(url);


            String search_html = GetBaikeHtml(url);  //获取搜索页面html
            //		System.out.println(search_html);

            //===============临时输出==================
            if (search_html.IndexOf("imgzd") > 0)
                Console.WriteLine("百度拦截");

            if (search_html.IndexOf("抱歉，没有找到与") > 0)
            {
                Console.WriteLine("百度百科尚未收录词条");
                terms_url_list.Add("NIL");
                return terms_url_list;
            }
            else
            {
                //临时输出
                Console.WriteLine("有义项");

                NSoup.Nodes.Document doc_sereach = NSoup.NSoupClient.Parse(search_html, "UTF-8");
                String g1 = ".mod-list>table>tbody>tr>td>h2>a"; //获取位置
                String g2 = "abs:href"; //获取URL规则

                NSoup.Select.Elements links = doc_sereach.Select(g1);
                List<String> sereach_url_list = new List<String>();

                foreach (NSoup.Nodes.Element link in links)
                {
                    String sereach_url = link.Attr(g2);
                    sereach_url_list.Add(sereach_url);
                }

                String target_url = sereach_url_list[0];  //只获取搜索后页面第一个
                String target_html = GetBaikeHtml(target_url);  //获取搜索后第一个页面文本

                if (target_html.IndexOf("多义词") > 0 && target_html.IndexOf("个义项") > 0)  //如果百度百科多义词
                {
                    //临时输出
                    Console.WriteLine("是多义词");
                    NSoup.Nodes.Document doc_target = NSoup.NSoupClient.Parse(target_html, "UTF-8");

                    String t1 = ".polysemeHeadLeft>a"; //获取位置
                    String t2 = "href"; //获取URL规则

                    NSoup.Select.Elements target_links = doc_target.Select(t1);
                    String postfix_terms = target_links[2].Attr(t2);  //获取义项列表页面URL后缀
                    String terms_url = "http://baike.baidu.com" + postfix_terms;
                    String terms_html = GetBaikeHtml(terms_url);

                    //临时输出
                    //				System.out.println(terms_url);
                    NSoup.Nodes.Document doc_term_list = NSoup.NSoupClient.Parse(terms_html, "UTF-8");
                    String tt1 = ".custom_dot>li>p>a"; //获取位置
                    String tt2 = "href"; //获取URL规则d
                    NSoup.Select.Elements terms_links = doc_term_list.Select(tt1);

                    //临时输出
                    //				System.out.println(terms_links.size());
                    foreach (NSoup.Nodes.Element term in terms_links)
                    {
                        String postfix_term = term.Attr(tt2);
                        String term_url = "http://baike.baidu.com" + postfix_term;

                        //临时输出
                        //					System.out.println(term_url);
                        terms_url_list.Add(term_url);
                    }

                    return terms_url_list;

                }
                else  //百度百科 单一词
                {
                    terms_url_list.Add(target_url);
                    return terms_url_list;
                }

            }

        }

        //static void Main(String[] args)
        //{
        //    String word = "中国";
        //    List<String> list = GetTermsURL(word);

        //    foreach (String s in list)
        //        Console.WriteLine(s);

        //    Console.ReadKey();

        //}

    }
}
