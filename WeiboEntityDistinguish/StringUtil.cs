using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiboEntityDistinguish
{
    class StringUtil
    {
        public static String RemoveBlank(String str)
        {
            String tmp_str = str.Trim();
            tmp_str = tmp_str.Replace("[", "");
            tmp_str = tmp_str.Replace("]", "");
            tmp_str = tmp_str.Replace("\u3000", " ");
            tmp_str = tmp_str.Replace(" ", "");
            tmp_str = tmp_str.Replace("　", "");
            String test = tmp_str.Trim();

            return test;
        
        }
        public static bool isChinese(char c)
        {
            if ((c >= 19968 && c <= 40869) && c != '·' && c != '/')
                return true;
            else
                return false;
        }


        public static bool isChinese(String str)
        {
            for (int i = 0; i < str.Count(); i++)
                if (!isChinese(str[i]))
                    return false;
            return true;
        }

        public static bool isPureLetter(String str)
        {
            for (int i = 0; i < str.Length; i++)
                if (str[i]<'a'||(str[i]>'z'&&str[i]<'A')||str[i]>'Z')
                    return false;

            return true;
        
        }

        public static List<String> GetNouns(String str)
	    {
            String remove_blank_str = RemoveBlank(str);
            String[] word_list = remove_blank_str.Split(' ');
     
		    List<String> nouns=new List<String>();

		    //---临时输出----
            //foreach (String s in word_list)
            //    Console.WriteLine("|" + s + "|");
	
		    String tmp_symbol;
		    String tmp_noun;

            //Console.WriteLine("开始==================");

            for (int i = 0; i < word_list.Count(); i++)
		    {
                if (word_list[i].Length >= 3 && word_list[i].IndexOf("/")>=1)
               {
                   //Console.WriteLine(word_list[i]);
                   tmp_noun=word_list[i].Substring(0,word_list[i].IndexOf("/")).Trim();
			       tmp_symbol=word_list[i].Substring(word_list[i].IndexOf("/")).Trim();

             
                	if((tmp_symbol.IndexOf("n")>=0&&isChinese(tmp_noun)&&tmp_noun.Length>=1)
                        ||(isPureLetter(tmp_noun)&&tmp_noun.Length>=1))
				             nouns.Add(tmp_noun);
               
               }
		
	    	}
            //Console.WriteLine("结束======================");
		    return nouns;
	    }


     
        //static void Main(String[] args)
        //{

        //    //		System.out.println(isPureLetter(" Google"));
        //    String str_input = "　　埃里克-斯波尔斯特拉 　　不管他手下是勒布朗-詹姆斯还是伯纳德-";
        //    String result = NlpirTest.GetNLPParseResult(str_input);
        //    Console.WriteLine(result);
        //    Console.WriteLine();

        //    foreach (String e in getNouns(result))
        //        Console.Write(e + " ");

        //    Console.ReadKey();

        //}


    }
}
