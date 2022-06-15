using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VectorShop.Models
{
    public class PriCat
    {
        public int PriCatId { get; set; }

        [Display(Name="عنوان دسته بندی")]
        public string PriCatTitle { get; set; }

        [Display(Name = "توضیحات دسته بندی")]
        public string PriCatDesc { get; set; }

        public virtual ICollection<SecCat> SecCats { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<NewsLetter> NewsLetters { get; set; }
        

    }
}