using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VectorShop.Models.ViewModels
{
    public class ContactViewModel
    {
        public int ContactId { get; set; }

        [Required(ErrorMessage = "پر کردن این بخش الزامی می باشد.")]
        [DataType(DataType.DateTime)]
        public DateTime ContactDate { get; set; }

        [Required(ErrorMessage = "پر کردن این بخش الزامی می باشد.")]
        [MaxLength(200, ErrorMessage = "مقدار فیلد نمی تواند از 200 کراکتر بیشتر باشد.")]
        [Display(Name="نام و نام خانوادگی")]
        public string ContactName { get; set; }

        [Required(ErrorMessage = "پر کردن این بخش الزامی می باشد.")]
        [MaxLength(200, ErrorMessage = "مقدار فیلد نمی تواند از 200 کراکتر بیشتر باشد.")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمی باشد.")]
        [Display(Name = "ایمیل")]
        public string ContactEmail { get; set; }

        [MaxLength(200, ErrorMessage = "مقدار فیلد نمی تواند از 200 کراکتر بیشتر باشد.")]
        [Display(Name = "شماره تلفن")]
        public string ContactPhone { get; set; }

        [Required(ErrorMessage = "پر کردن این بخش الزامی می باشد.")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "توضیحات")]
        public string ContactBody { get; set; }

    }
}