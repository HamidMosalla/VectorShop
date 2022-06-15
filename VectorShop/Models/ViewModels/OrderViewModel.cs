using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VectorShop.Models.ViewModels
{
    public class OrderViewModel
    {
        //public int OrderId { get; set; } Don't need this on ViewModel

        [Required(ErrorMessage = "پر کردن این بخش الزامی می باشد.")]
        [MaxLength(400, ErrorMessage = "مقدار فیلد نمی تواند از 400 کراکتر بیشتر باشد.")]
        public string OrderTitle { get; set; }

        [Required(ErrorMessage = "پر کردن این بخش الزامی می باشد.")]
        [DataType(DataType.DateTime)]
        public DateTime OrderDate { get; set; }

        [MaxLength(200, ErrorMessage = "مقدار فیلد نمی تواند از 200 کراکتر بیشتر باشد.")]
        [Required(ErrorMessage = "پر کردن این بخش الزامی می باشد.")]
        public string OrderFirstName { get; set; }

        [MaxLength(200, ErrorMessage = "مقدار فیلد نمی تواند از 200 کراکتر بیشتر باشد.")]
        [Required(ErrorMessage = "پر کردن این بخش الزامی می باشد.")]
        public string OrderLastName { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "لطفا یک شماره تلفن معتبر وارد نمایید.")]
        public string OrderPhoneNumber { get; set; }

        [MaxLength(200, ErrorMessage = "مقدار فیلد نمی تواند از 200 کراکتر بیشتر باشد.")]
        [Required(ErrorMessage = "پر کردن این بخش الزامی می باشد.")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمی باشد.")]
        public string OrderEmail { get; set; }

        public bool OrderIsSuccessful { get; set; }

        [Required(ErrorMessage = "پر کردن این بخش الزامی می باشد.")]
        [Range(0.0, double.MaxValue, ErrorMessage = "لطفا یک عدد معتبر وارد نمایید.")]
        public decimal OrderPrice { get; set; }

        [Required(ErrorMessage = "پر کردن این بخش الزامی می باشد.")]
        public int ProductIDfk { get; set; }
        //[ForeignKey("ProductIDfk")]   
        public virtual Product Product { get; set; }


        [Required(ErrorMessage = "پر کردن این بخش الزامی می باشد.")]
        public int PriCatIDfk { get; set; }
        //[ForeignKey("PriCatIDfk")]   
        public virtual PriCat PriCat { get; set; }


        [Required(ErrorMessage = "پر کردن این بخش الزامی می باشد.")]
        public int SecCatIDfk { get; set; }
        //[ForeignKey("SecCatIDfk")]   
        public virtual SecCat SecCat { get; set; }




    }
}