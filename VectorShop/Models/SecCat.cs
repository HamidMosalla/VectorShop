using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VectorShop.Models
{
    public class SecCat
    {

        public int SecCatId { get; set; }

        [Display(Name = "عنوان دسته بندی")]
        public string SecCatTitle { get; set; }

        [Display(Name = "توضیحات دسته بندی")]
        public string SecCatDesc { get; set; }


        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<NewsLetter> NewsLetters { get; set; }

        public int PriCatIDfk { get; set; }
        //[ForeignKey("PriCatIDfk")]   
        public virtual PriCat PriCat { get; set; }
    }
}