using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VectorShop.Models.ViewModels
{
    public class SlideShowViewModel
    {
        public int SlideShowId { get; set; }

        [Required(ErrorMessage = "پر کردن این بخش الزامی می باشد.")]
        [MaxLength(200, ErrorMessage = "مقدار فیلد نمی تواند از 200 کراکتر بیشتر باشد.")]
        [Display(Name="عنوان اسلاید")]
        public string SlideShowTitle { get; set; }

        [MaxLength(400, ErrorMessage = "مقدار فیلد نمی تواند از 400 کراکتر بیشتر باشد.")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "توضیحات اسلاید")]
        public string SlideShowDescription { get; set; }

        [Required(ErrorMessage = "پر کردن این بخش الزامی می باشد.")]
        [Display(Name = "عکس اسلاید")]
        public string SlideShowPictrure { get; set; }

        [Required(ErrorMessage = "پر کردن این بخش الزامی می باشد.")]
        //[DataType(DataType.Url)]
        [Display(Name = "لینک اسلاید")]
        [DataType(DataType.MultilineText)]
        public string SlideShowLink { get; set; }


    }
}