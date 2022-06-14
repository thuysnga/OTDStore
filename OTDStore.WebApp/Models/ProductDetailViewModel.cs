using OTDStore.ViewModels.Catalog.Brands;
using OTDStore.ViewModels.Catalog.Categories;
using OTDStore.ViewModels.Catalog.ProductImages;
using OTDStore.ViewModels.Catalog.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OTDStore.WebApp.Models
{
    public class ProductDetailViewModel
    {
        public CategoryVM Category { get; set; }

        public BrandVM Brand { get; set; }

        public ProductVM Product { get; set; }

        public List<ProductVM> RelatedProducts { get; set; }

        public List<ProductImageViewModel> ProductImages { get; set; }
    }
}
