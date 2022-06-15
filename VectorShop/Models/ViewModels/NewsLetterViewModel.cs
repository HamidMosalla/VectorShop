using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VectorShop.Models.ViewModels
{
    public class NewsLetterViewModel
    {
        public int NewsLetterId { get; set; }

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "پر کردن این بخش الزامی می باشد.")]
        [MaxLength(200, ErrorMessage = "مقدار فیلد نمی تواند از 200 کراکتر بیشتر باشد.")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمی باشد.")]
        public string NewsLetterEmail { get; set; }

        [Required(ErrorMessage = "پر کردن این بخش الزامی می باشد.")]
        [Display(Name = "نام و نام خانوادگی")]
        [MaxLength(200, ErrorMessage = "مقدار فیلد نمی تواند از 200 کراکتر بیشتر باشد.")]
        public string NewsLetterSubscriberName { get; set; }

        [Display(Name = "انتخاب بودن عضویت")]
        public bool IsSelected { get; set; }

        [Display(Name = "فعال بودن عضویت")]
        public bool IsActive { get; set; }

        [Display(Name = "انتخاب علاقه مندی اصلی")]
        public int? PriCatIDfk { get; set; }
        //[ForeignKey("PriCatIDfk")]  
        public virtual PriCat PriCat { get; set; }

        [Display(Name = "انتخاب علاقه مندی فرعی")]
        public int? SecCatIDfk { get; set; }
        //[ForeignKey("SecCatIDfk")]  
        public virtual SecCat SecCat { get; set; }


    }
}