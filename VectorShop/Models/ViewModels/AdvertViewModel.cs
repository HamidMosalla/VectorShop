using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VectorShop.Models.ViewModels
{
    public class AdvertViewModel
    {

        public int AdvertId { get; set; }


        [Required(ErrorMessage = "پر کردن این بخش الزامی می باشد.")]
        [DataType(DataType.DateTime)]
        public DateTime AdvertDateAdded { get; set; }

        [MaxLength(200,ErrorMessage="مقدار فیلد نمی تواند از 200 کراکتر بیشتر باشد.")]
        [Required(ErrorMessage = "پر کردن این بخش الزامی می باشد.")]
        [Display(Name="عنوان تبلیغات")]
        public string AdvertTitle { get; set; }

        [Required(ErrorMessage = "پر کردن این بخش الزامی می باشد.")]
        [Display(Name = "اولویت تبلیغات")]
        [Range(0, int.MaxValue, ErrorMessage = "لطفا یک عدد معتبر وارد نمایید.")]
        public int AdvertPriority { get; set; }

        [Display(Name = "لینک تبلیغات")]
        [DataType(DataType.MultilineText)]
        public string AdvertUrl { get; set; }

        [Required(ErrorMessage = "پر کردن این بخش الزامی می باشد.")]
        [Display(Name = "تصویر تبلیغات")]
        public string AdvertPicture { get; set; }

    }
}