using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VectorShop.Models.ViewModels
{
    public class NewDesignOrderViewModel
    {
        public int NewDesignOrderId { get; set; }

        [Required(ErrorMessage = "پر کردن این بخش الزامی می باشد.")]
        [MaxLength(200, ErrorMessage = "مقدار فیلد نمی تواند از 200 کراکتر بیشتر باشد.")]
        [Display(Name="عنوان سفارش")]
        public string NewDesignOrderTitle { get; set; }

        [Required(ErrorMessage = "پر کردن این بخش الزامی می باشد.")]
        [DataType(DataType.DateTime)]
        public DateTime NewDesignOrderDate { get; set; }

        [Required(ErrorMessage = "پر کردن این بخش الزامی می باشد.")]
        [MaxLength(200, ErrorMessage = "مقدار فیلد نمی تواند از 200 کراکتر بیشتر باشد.")]
        [Display(Name = "نام و نام خانوادگی")]
        public string NewDesignOrderName { get; set; }

        [Required(ErrorMessage = "پر کردن این بخش الزامی می باشد.")]
        [MaxLength(200, ErrorMessage = "مقدار فیلد نمی تواند از 200 کراکتر بیشتر باشد.")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمی باشد.")]
        [Display(Name = "ایمیل")]
        public string NewDesignOrderEmail { get; set; }

        [MaxLength(200, ErrorMessage = "مقدار فیلد نمی تواند از 350 کراکتر بیشتر باشد.")]
        [Display(Name = "وب سایت")]
        [DataType(DataType.Url)]
        public string NewDesignOrderWebSite { get; set; }

        [MaxLength(200, ErrorMessage = "مقدار فیلد نمی تواند از 400 کراکتر بیشتر باشد.")]
        [Display(Name = "آدرس محل زندگی")]
        [DataType(DataType.MultilineText)]
        public string NewDesignOrderAddress { get; set; }


        [Required(ErrorMessage = "پر کردن این بخش الزامی می باشد.")]
        [MaxLength(4000, ErrorMessage = "مقدار فیلد نمی تواند از 4000 کراکتر بیشتر باشد.")]
        [Display(Name = "توضیحات سفارش")]
        [DataType(DataType.MultilineText)]
        public string NewDesignOrderDescBody { get; set; }

        [Display(Name = "عکس نمونه برای سفارش")]
        public string NewDesignOrderPicture { get; set; }

        [Required(ErrorMessage = "پر کردن این بخش الزامی می باشد.")]
        [Display(Name = "شماره تماس")]
        public string NewDesignOrderPhone { get; set; }

        [Required(ErrorMessage = "پر کردن این بخش الزامی می باشد.")]
        [Range(0.0, double.MaxValue, ErrorMessage = "لطفا یک عدد معتبر وارد نمایید.")]
        [Display(Name = "حداکثر بودجه برای طرح مورد نظر")]
        public decimal NewDesignOrderMaxAffordablePrice { get; set; }

        [Required(ErrorMessage = "پر کردن این بخش الزامی می باشد.")]
        [Display(Name = "طریقه ی آشنایی با ما")]
        public string NewDesignOrderHowFindUs { get; set; }







    }
}