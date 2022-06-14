using System;
using System.Collections.Generic;
using System.Text;

namespace OTDStore.Data.Entities
{
    public class Product
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public string Insurance { get; set; }
        public string Description { set; get; }
        public string YearRelease { set; get; }
        public string Color { set; get; }
        public string CPU { set; get; }
        public string VGA { set; get; }
        public string Memory { set; get; }
        public string RAM { set; get; }
        public string Camera { set; get; }
        public string Bluetooth { get; set; }
        public string Monitor { get; set; }
        public string Battery { set; get; }
        public string Size { set; get; }
        public string OS { set; get; }
        public decimal Price { set; get; }
        public decimal OriginalPrice { set; get; }
        public int Stock { set; get; }
        public int ViewCount { set; get; }
        public DateTime DateCreated { set; get; }
        public bool? IsFeature { get; set; }

        public List<ProductInCategory> ProductInCategories { get; set; }
        public List<ProductInBrand> ProductInBrands { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
        public List<Cart> Carts { get; set; }
        public List<ProductImage> ProductImages { get; set; }
    }
}
