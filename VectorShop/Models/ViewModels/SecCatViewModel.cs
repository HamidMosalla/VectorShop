using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VectorShop.Models.ViewModels
{
    public class SecCatViewModel
    {

        //public int SecCatId { get; set; } Don't need this on ViewModel
        //public virtual ICollection<Product> Products { get; set; }
        //public virtual ICollection<Order> Orders { get; set; }

        [Required(ErrorMessage = "پر کردن این بخش الزامی می باشد.")]
        [MaxLength(400, ErrorMessage = "مقدار فیلد نمی تواند از 400 کراکتر بیشتر باشد.")]
        public string SecCatTitle { get; set; }

        [Required(ErrorMessage = "پر کردن این بخش الزامی می باشد.")]
        [MaxLength(400, ErrorMessage = "مقدار فیلد نمی تواند از 400 کراکتر بیشتر باشد.")]
        public string SecCatDesc { get; set; }

        [Required(ErrorMessage = "پر کردن این بخش الزامی می باشد.")]
        public int PriCatIDfk { get; set; }
        //[ForeignKey("PriCatIDfk")]   
        public virtual PriCat PriCat { get; set; }
    }
}