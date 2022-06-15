using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VectorShop.Models.ViewModels
{
    public class PriCatViewModel
    {
        public string PriCatId { get; set; } //Changed it to string because dropdownList posted value is of type string.
        //public virtual ICollection<SecCat> SecCats { get; set; }
        //public virtual ICollection<Product> Products { get; set; }
        //public virtual ICollection<Order> Orders { get; set; }

        [Required(ErrorMessage = "پر کردن این بخش الزامی می باشد.")]
        [MaxLength(400, ErrorMessage = "مقدار فیلد نمی تواند از 400 کراکتر بیشتر باشد.")]
        [Display(Name="عنوان دسته بندی")]
        public string PriCatTitle { get; set; }

        [Required(ErrorMessage = "پر کردن این بخش الزامی می باشد.")]
        [DataType(DataType.MultilineText)]
        [MaxLength(400, ErrorMessage = "مقدار فیلد نمی تواند از 400 کراکتر بیشتر باشد.")]
        [Display(Name = "توضیحات دسته بندی دسته بندی")]
        public string PriCatDesc { get; set; }

        
        

    }
}