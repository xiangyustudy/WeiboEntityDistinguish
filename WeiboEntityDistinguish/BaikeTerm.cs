using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiboEntityDistinguish
{
    class BaikeTerm
    {
        public BaikeTerm()
        {

        }

        public BaikeTerm(String word, String title, String url, String html, String description, double match)
        {
            this.Word = word;
            this.Title = title;
            this.Url = url;
            this.Html = html;
            this.Description = description;
            this.Match = match;
        }

        public String Word { get; set; }
        public String Title { get; set; }
        public String Url { get; set; }
        public String Html { get; set; }
        public String Description { get; set; }
        public double Match { get; set; }
        public String MatchStr
        {
            get
            {
                return Match.ToString("N4");
            }
        }

    }
}
