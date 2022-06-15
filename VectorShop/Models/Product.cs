using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VectorShop.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductPicture { get; set; }
        public string ProductDescription { get; set; }
        public bool IsProductFree { get; set; }
        public bool IsProductIsInIndex { get; set; }
        public string ProductDownloadLink { get; set; }
        public DateTime ProductDate { get; set; }
        public decimal ProductPrice { get; set; }
        public virtual ICollection<Order> Orders { get; set; }

        public int? PriCatIDfk { get; set; }
        //[ForeignKey("PriCatIDfk")]   
        public virtual PriCat PriCat { get; set; }


        public int? SecCatIDfk { get; set; }
        //[ForeignKey("SecCatIDfk")]   
        public virtual SecCat SecCat { get; set; }

    }
}