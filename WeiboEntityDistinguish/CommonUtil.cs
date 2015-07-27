using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiboEntityDistinguish
{
    class CommonUtil
    {
        //计算两个List的Jaccard距离
        public static double getJaccardDistance(List<string> source, List<string> target)
        {
            HashSet<string> set1 = new HashSet<string>(source);
            HashSet<string> set2 = new HashSet<string>(target);

            HashSet<string> intersection = new HashSet<string>(set1);
            HashSet<string> union = new HashSet<string>(set1);

            intersection.IntersectWith(set2);
            union.UnionWith(set2);

            return intersection.Count / (double)union.Count;
        }

        //对数组进行归一化处理
        public static double[] getNormalization(int[] a)
        {
            int max = a[0];
            int min = a[0];

            double[] normalization = new double[a.Count()];

            for (int i = 0; i < a.Count(); i++)
            {
                max = max > a[i] ? max : a[i];
                min = min < a[i] ? min : a[i];
            }


            for (int i = 0; i < a.Count(); i++)
            {
                normalization[i] = (a[i] - min) / (double)(max - min);
            }

            return normalization;
        }

        public static double GetCosdistance(Dictionary<String, int> weibo_dic,Dictionary<String, int> entity_dic)
        {
            if (weibo_dic.Count == 0 || entity_dic.Count == 0)
            {
                return 0.0;
            }
            else
            {
                List<String> union_list = new List<String>();
                foreach (var map in weibo_dic) //获取weibo_dic所有词语
                {
                    union_list.Add(map.Key);
                }

                foreach (var map in entity_dic) //获取entity_dic所有词语
                {
                    if (!union_list.Contains(map.Key))
                        union_list.Add(map.Key);
                }

                int len = union_list.Count();
                //  新建词向量 
                int[] vector_weibo = new int[len];
                int[] vector_entity = new int[len];

                for (int i = 0; i < len; i++)
                {
                    String word = union_list[i];

                    if (weibo_dic.ContainsKey(word))
                        vector_weibo[i] = weibo_dic[word];
                    else
                        vector_weibo[i] = 0;
                }


                for (int i = 0; i < len; i++)
                {
                    String word = union_list[i];

                    if (entity_dic.ContainsKey(word))
                        vector_entity[i] = entity_dic[word];
                    else
                        vector_entity[i] = 0;
                }

                int fenzi = 0, len1 = 0, len2 = 0;

                for (int i = 0; i < len; i++)
                {
                    fenzi += vector_weibo[i] * vector_entity[i];
                    len1 += vector_weibo[i] * vector_weibo[i];
                    len2 += vector_entity[i] * vector_entity[i];
                }

                return (double)fenzi / (double)(Math.Sqrt(len1) * Math.Sqrt(len2));
            }
        
        }



        //临时的测试
        static void Main(string[] args)
        {
            Dictionary<String,int> dic1=new Dictionary<string,int>();
            dic1.Add("A",2);
            dic1.Add("B",3);
            dic1.Add("C",4);

            Dictionary<String, int> dic2 = new Dictionary<string, int>();
            dic2.Add("C", 7);
            dic2.Add("E", 2);
            dic2.Add("F", 6);

            Console.WriteLine(GetCosdistance(dic1,dic2));

            Console.ReadKey();
        }

    }
}
