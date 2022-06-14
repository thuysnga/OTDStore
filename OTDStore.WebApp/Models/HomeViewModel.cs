using OTDStore.ViewModels.Catalog.Products;
using OTDStore.ViewModels.Utilities.Slides;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OTDStore.WebApp.Models
{
    public class HomeViewModel
    {
        public List<SlideVM> Slides { get; set; }

        public List<ProductVM> Products { get; set; }
    }
}
