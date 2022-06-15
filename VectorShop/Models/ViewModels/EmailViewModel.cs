using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VectorShop.Models.ViewModels
{
    public class EmailViewModel
    {
        [Required(ErrorMessage = "پر کردن این بخش الزامی می باشد.")]
        [Display(Name="عنوان ایمیل")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "پر کردن این بخش الزامی می باشد.")]
        [Display(Name = "متن ایمیل")]
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }

    }
}