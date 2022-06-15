using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VectorShop.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string OrderTitle { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderFirstName { get; set; }
        public string OrderLastName { get; set; }
        public string OrderPhoneNumber { get; set; }
        public string OrderEmail { get; set; }
        public bool OrderIsSuccessful { get; set; }
        public decimal OrderPrice { get; set; }


        public int ProductIDfk { get; set; }
        //[ForeignKey("ProductIDfk")]   
        public virtual Product Product { get; set; }



        public int? PriCatIDfk { get; set; }
        //[ForeignKey("PriCatIDfk")]   
        public virtual PriCat PriCat { get; set; }


        public int? SecCatIDfk { get; set; }
        //[ForeignKey("SecCatIDfk")]   
        public virtual SecCat SecCat { get; set; }




    }
}