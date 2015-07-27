using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiboEntityDistinguish
{
    class Process
    {
        public static List<BaikeTerm> GetMatchBaikeTerm(String word, String weibo)
        {
            List<BaikeTerm> bt_list = new List<BaikeTerm>();
            List<String> terms_url_list = SpiderUtil.GetTermsURL(word);

            if (terms_url_list.Count() == 1 ) 
            {

                if (terms_url_list[0].Equals("NIL")) //如果百度百科没有收录
                {
                    return null;
                }
                else  //如果在百度百科只有一个义项
                {

                    String url = terms_url_list[0];
                    String html = SpiderUtil.GetBaikeHtml(url);
                    String title = SpiderUtil.GetBaikeExplatationTitle(html);
                    String description = SpiderUtil.GetDescription(html);

                    BaikeTerm bt = new BaikeTerm(word, title, url, html, description, 0.0);
                    bt_list.Add(bt);

                    return bt_list;

                }
            
            }
            else //如果在百科中有多个义项
            {
                Dictionary<String, int> weibo_dic = GetTextFrequencyDictionary(weibo);  //获取微博统计dic
                weibo_dic.Remove(word); //微博统计dic要移除待测词语本身

                int size = terms_url_list.Count();
                int match_is_zero = 0;

                foreach (String url in terms_url_list)
                {
                    String html = SpiderUtil.GetBaikeHtml(url);
                    String title = SpiderUtil.GetBaikeExplatationTitle(html);
                    String description = SpiderUtil.GetDescription(html);
                     
                    Dictionary<String, int> entity_dic = GetTextFrequencyDictionary(html);  //获取实体统计dic
                    entity_dic.Remove(word);  //实体统计dic也要移除待测词语本省

                    double match = CommonUtil.GetCosdistance(weibo_dic, entity_dic);

                    if (match == 0.0)
                        match_is_zero++;

                    BaikeTerm bt = new BaikeTerm(word, title, url, html, description, match);
                    bt_list.Add(bt);
                }

                if (match_is_zero == size)
                {
                    bt_list.Sort(
                    (Comparison<BaikeTerm>)delegate(BaikeTerm bt1, BaikeTerm bt2)
                    {
                        return bt2.Html.Length > bt1.Html.Length ? 1 : -1;
                     });
                }
                else
                {
                    bt_list.Sort(
                   (Comparison<BaikeTerm>)delegate(BaikeTerm bt1, BaikeTerm bt2)
                   {
                       return bt2.Match >=bt1.Match ? 1 : -1;
                   });
                   
                }
                return bt_list;
            }
        
        }

        public static Dictionary<String,int> GetTextFrequencyDictionary(String weibo)
        {
            Dictionary<String, int> dic = new Dictionary<string, int>();
            String nlpir = NlpirUtil.GetNLPParseResult(weibo); //获取分词结果
            List<String> nouns_list = StringUtil.GetNouns(nlpir);//获取名词组

            foreach (String noun in nouns_list)
            {
                if (dic.ContainsKey(noun))
                    dic[noun]++; //直接++ 操作增加
                else 
                    dic.Add(noun,1);
            
            }

            return dic;
    
        }

        //static void Main(String[] args)
        //{
        //    String weibo = "";
        //    String word = "";
        //}
    }
}