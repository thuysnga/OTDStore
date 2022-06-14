using OTDStore.ViewModels.Catalog.Brands;
using OTDStore.ViewModels.Catalog.Categories;
using OTDStore.ViewModels.Catalog.ProductImages;
using System.Collections.Generic;

namespace OTDStore.ViewModels.Catalog.Products
{
    public class ProductDetailVM
    {
        public CategoryVM Category { get; set; }

        public BrandVM Brand { get; set; }

        public ProductVM Product { get; set; }

        public List<ProductVM> RelatedProducts { get; set; }

        public List<ProductImageViewModel> ProductImages { get; set; }
    }
}
