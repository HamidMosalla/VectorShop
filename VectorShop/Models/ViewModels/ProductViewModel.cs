using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VectorShop.Models.ViewModels
{
    public class ProductViewModel
    {
        
        //public virtual ICollection<Order> Orders { get; set; }

        public int ProductId { get; set; }

        [Required(ErrorMessage = "پر کردن این بخش الزامی می باشد.")]
        [MaxLength(70, ErrorMessage = "مقدار فیلد نمی تواند از 70 کراکتر بیشتر باشد.")]
        [Display(Name="نام محصول")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "پر کردن این بخش الزامی می باشد.")]
        [Display(Name = "عکس محصول")]
        public string ProductPicture { get; set; }

        [Required(ErrorMessage = "پر کردن این بخش الزامی می باشد.")]
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(Name = "توضیحات محصول")]
        public string ProductDescription { get; set; }

        [Display(Name = "رایگان بودن محصول")]
        public bool IsProductFree { get; set; }

        [Display(Name = "نمایش در صفحه اصلی")]
        public bool IsProductIsInIndex { get; set; }

        [Required(ErrorMessage = "پر کردن این بخش الزامی می باشد.")]
        [Display(Name = "انتخاب فایل محصول")]
        public string ProductDownloadLink { get; set; }

        [Required(ErrorMessage = "پر کردن این بخش الزامی می باشد.")]
        [Range(0.0, double.MaxValue, ErrorMessage = "لطفا یک عدد معتبر وارد نمایید.")]
        [Display(Name = "قیمت محصول")]
        public decimal ProductPrice { get; set; }

        
        [Display(Name = "دسته بندی اصلی")]
        public int? PriCatIDfk { get; set; }
        //[ForeignKey("PriCatIDfk")]   
        public virtual PriCat PriCat { get; set; }

        [Display(Name = "دسته بندی فرعی")]
        public int? SecCatIDfk { get; set; }
        //[ForeignKey("SecCatIDfk")]   
        public virtual SecCat SecCat { get; set; }

    }
}