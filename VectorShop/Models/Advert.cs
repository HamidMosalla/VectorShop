using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VectorShop.Models
{
    public class Advert
    {

        public int AdvertId { get; set; }
        public DateTime AdvertDateAdded { get; set; }
        public string AdvertTitle { get; set; }
        public int AdvertPriority { get; set; }
        public string AdvertUrl { get; set; }
        public string AdvertPicture { get; set; }

    }
}