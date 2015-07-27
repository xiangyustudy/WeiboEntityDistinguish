using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiboEntityDistinguish
{
    class BaikeItem
    {
            private String preprocess;
	        private String explanation;
	
	        private String title;
	        private String url;
	        private String description;
	        private String labs;
	
	        private int query;
	        private double lab_similar;
	        private double match;

	        public BaikeItem()
	        {
		
	        }


	        public double getLab_similar() {
		        return lab_similar;
	        }


	        public void setLab_similar(double lab_similar) {
		        this.lab_similar = lab_similar;
	        }



	        public double getMatch() {
		        return match;
	        }



	        public void setMatch(double match) {
		        this.match = match;
	        }



	        public BaikeItem(String preprocess,String explanation,String title,String url,String description,String labs,int query)
	        {
		        this.preprocess=preprocess;
		        this.explanation=explanation;
		        this.title=title;
		        this.url=url;
		        this.description=description;
		        this.labs=labs;
		        this.query=query;
		
	        }
	        public String getPreprocess() {
		        return preprocess;
	        }

	        public void setPreprocess(String preprocess) {
		        this.preprocess = preprocess;
	        }

	        public String getExplanation() {
		        return explanation;
	        }

	        public void setExplanation(String explanation) {
		        this.explanation = explanation;
	        }

	        public String getTitle() {
		        return title;
	        }

	        public void setTitle(String title) {
		        this.title = title;
	        }

	        public String getUrl() {
		        return url;
	        }

	        public void setUrl(String url) {
		        this.url = url;
	        }

	        public String getDescription() {
		        return description;
	        }

	        public void setDescription(String description) {
		        this.description = description;
	        }

	        public String getLabs() {
		        return labs;
	        }

	        public void setLabs(String labs) {
		        this.labs = labs;
	        }

	        public int getQuery() {
		        return query;
	        }

	        public void setQuery(int query) {
		        this.query = query;
	        }

	
	        public String toString() {
		        return "Item [preprocess=" + preprocess + ", explanation="
				        + explanation + ", title=" + title + ", url=" + url
				        + ", description=" + description + ", labs=" + labs
				        + ", query=" + query + "]";
	        }


    }
}
