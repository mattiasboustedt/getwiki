using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GetWiki.Models
{
    public class Article
    {
        /*
         * ArticleID is used to identify an object of the class Article. 
         */
        public int ArticleID { get; set; }

        /*
         * WikArticleId is the key to a specific article on Wikipedia. 
         * This is the key to retrieve an article later on. 
         */
        public int WikiArticleId { get; set; }
        public int Ns { get; set; }
        public string Title { get; set; }

        /*
         * Date and time when the article was fethced from wikipedia. Used for sorting.
         */
        public DateTime TimeFetched { get; set; }
        public int? CategoryID { get; set; }

        public virtual Category Category { get; set; }
    }
}