using System;
using System.Collections.Generic;
using System.Text;

namespace OTDStore.ViewModels.Sales
{
    public class GetOrderDetailRequest
    {
        public int Id { get; set; }

        public Guid UserId { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public decimal Total { get; set; }

        public string PaymentMethod { get; set; }

        public List<OrderDetailVM> OrderDetails { set; get; } = new List<OrderDetailVM>();
    }
}
