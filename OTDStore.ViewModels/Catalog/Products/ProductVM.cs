using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OTDStore.ViewModels.Catalog.Products
{
    public class ProductVM
    {
        [Display(Name = "Mã")]
        public int Id { set; get; }

        [Display(Name = "Tên sản phẩm")]
        public string Name { set; get; }

        [Display(Name = "Bảo hành")]
        public string Insurance { get; set; }

        [Display(Name = "Mô tả")]
        public string Description { set; get; }

        [Display(Name = "Năm ra mắt")]
        public string YearRelease { set; get; }

        [Display(Name = "Màu sắc")]
        public string Color { set; get; }

        [Display(Name = "CPU")]
        public string CPU { set; get; }

        [Display(Name = "VGA")]
        public string VGA { set; get; }

        [Display(Name = "Dung lượng")]
        public string Memory { set; get; }

        [Display(Name = "RAM")]
        public string RAM { set; get; }

        [Display(Name = "Camera")]
        public string Camera { set; get; }

        [Display(Name = "Bluetooth")]
        public string Bluetooth { get; set; }

        [Display(Name = "Màn hình")]
        public string Monitor { get; set; }

        [Display(Name = "Pin")]
        public string Battery { set; get; }

        [Display(Name = "Kích thước")]
        public string Size { set; get; }

        [Display(Name = "Hệ điều hành")]
        public string OS { set; get; }

        [Display(Name = "Giá khuyến mãi")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}")]
        public decimal Price { set; get; }

        [Display(Name = "Giá gốc")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}")]
        public decimal OriginalPrice { set; get; }

        [Display(Name = "Số lượng")]
        public int Stock { set; get; }

        [Display(Name = "Lượt xem")]
        public int ViewCount { set; get; }

        [Display(Name = "Ngày tạo")]
        public DateTime DateCreated { set; get; }
        public bool? IsFeature { get; set; }

        [Display(Name = "Ảnh đại diện")]
        public string ThumbnailImage { get; set; }
        public List<string> Categories { get; set; } = new List<string>();
        public List<string> Brands { get; set; } = new List<string>();
    }
}