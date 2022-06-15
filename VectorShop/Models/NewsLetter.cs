using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VectorShop.Models
{
    public class NewsLetter
    {
        public int NewsLetterId { get; set; }
        public string NewsLetterEmail { get; set; }
        public string NewsLetterSubscriberName { get; set; }
        public bool IsActive { get; set; }

        public bool IsSelected { get; set; }
        public int? PriCatIDfk { get; set; }
        //[ForeignKey("PriCatIDfk")]  
        public virtual PriCat PriCat { get; set; }

        public int? SecCatIDfk { get; set; }
        //[ForeignKey("SecCatIDfk")]  
        public virtual SecCat SecCat { get; set; }

    }
}