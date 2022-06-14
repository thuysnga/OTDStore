using OTDStore.Data.Entities;
using OTDStore.Data.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OTDStore.ViewModels.Sales
{
    public class OrderVM
    {
        [Display(Name = "Mã hóa đơn")]
        public int Id { set; get; }

        [Display(Name = "Ngày tạo")]
        public DateTime OrderDate { set; get; }

        [Display(Name = "Mã khách hàng")]
        public Guid UserId { set; get; }

        [Display(Name = "Tên người nhận")]
        public string ShipName { set; get; }

        [Display(Name = "Địa chỉ giao hàng")]
        public string ShipAddress { set; get; }

        [Display(Name = "Email người nhận")]
        public string ShipEmail { set; get; }

        [Display(Name = "SĐT người nhận")]
        public string ShipPhoneNumber { set; get; }

        [Display(Name = "Tổng tiền")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}")]
        public decimal Total { get; set; }

        [Display(Name = "Phương thức thanh toán")]
        public string PaymentMethod { get; set; }

        [Display(Name = "Trạng thái")]
        public OrderStatus Status { set; get; }
        public List<OrderDetailVM> OrderDetails { get; set; }
        public AppUser AppUser { get; set; }
    }
}
