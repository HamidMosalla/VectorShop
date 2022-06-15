using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VectorShop.Models.ViewModels
{
    public class ArticleViewModel
    {

        public int ArticleId { get; set; }


        [Required(ErrorMessage = "پر کردن این بخش الزامی می باشد.")]
        [MaxLength(70, ErrorMessage = "مقدار فیلد نمی تواند از 70 کراکتر بیشتر باشد.")]
        [Display(Name = "عنوان مقاله")]
        public string ArticleTitle { get; set; }

        [Required(ErrorMessage = "پر کردن این بخش الزامی می باشد.")]
        [DataType(DataType.DateTime)]
        public DateTime ArticleDate { get; set; }

        [Required(ErrorMessage = "پر کردن این بخش الزامی می باشد.")]
        [Display(Name="متن مقاله")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string ArticleBody { get; set; }


    }
}