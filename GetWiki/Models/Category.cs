using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GetWiki.Models
{
    public class Category
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }

        public virtual ICollection<Article> Articles { get; set; }
    }
}