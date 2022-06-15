using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VectorShop.Models
{
    public class Link
    {
        public int LinkId { get; set; }
        public int LinkPriority { get; set; }
        public string LinkTitle { get; set; }
        public string LinkUrl { get; set; }
        public string LinkDesc { get; set; }

    }
}