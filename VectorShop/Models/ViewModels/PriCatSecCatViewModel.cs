using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VectorShop.Models.ViewModels
{
    public class PriCatSecCatViewModel
    {
        public IEnumerable<PriCat> PriCat { get; set; }
        public IEnumerable<SecCat> SecCat { get; set; }
        public PriCat SPriCat { get; set; }
        public SecCat SSecCat { get; set; }
    }
}