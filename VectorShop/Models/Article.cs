using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VectorShop.Models
{
    public class Article
    {

        public int ArticleId { get; set; }
        public string ArticleTitle { get; set; }
        public DateTime ArticleDate { get; set; }
        public string ArticleBody { get; set; }


    }
}