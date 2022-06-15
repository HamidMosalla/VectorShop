using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VectorShop.Models.ViewModels
{
    public class LinkViewModel
    {
        public int LinkId { get; set; }

        [Required(ErrorMessage = "پر کردن این بخش الزامی می باشد.")]
        [Range(0, int.MaxValue, ErrorMessage = "لطفا یک عدد معتبر وارد نمایید.")]
        [Display(Name = "اولویت لینک")]
        public int LinkPriority { get; set; }

        [MaxLength(200, ErrorMessage = "مقدار فیلد نمی تواند از 200 کراکتر بیشتر باشد.")]
        [Required(ErrorMessage = "پر کردن این بخش الزامی می باشد.")]
        [Display(Name = "عنوان لینک")]
        public string LinkTitle { get; set; }


        [Required(ErrorMessage = "پر کردن این بخش الزامی می باشد.")]
        [DataType(DataType.Url)]
        [Display(Name = "آدرس لینک")]
        public string LinkUrl { get; set; }

        [MaxLength(200, ErrorMessage = "مقدار فیلد نمی تواند از 200 کراکتر بیشتر باشد.")]
        [Display(Name = "توضیحات لینک")]
        [DataType(DataType.MultilineText)]
        public string LinkDesc { get; set; }

    }
}