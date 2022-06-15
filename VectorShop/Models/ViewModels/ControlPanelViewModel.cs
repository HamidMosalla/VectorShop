using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VectorShop.Models.ViewModels
{
    public class ControlPanelViewModel
    {
        public int ControlPanelId { get; set; }

        [Range(4,10, ErrorMessage="تعداد اسلاید شوها نمی تواند کمتر از چهار و بیشتر از ده باشد.")]
        [Required(ErrorMessage = "پر کردن این بخش الزامی می باشد.")]
        [Display(Name="تعداد اسلاید شو")]
        public int SlideShowNumber { get; set; }

        [Required(ErrorMessage = "پر کردن این بخش الزامی می باشد.")]
        [Display(Name = "درباره")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string About { get; set; }
    }
}