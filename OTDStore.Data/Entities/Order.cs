﻿using OTDStore.Data.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace OTDStore.Data.Entities
{
    public class Order
    {
        public int Id { set; get; }
        public DateTime OrderDate { set; get; }
        public Guid UserId { set; get; }
        public string ShipName { set; get; }
        public string ShipAddress { set; get; }
        public string ShipEmail { set; get; }
        public string ShipPhoneNumber { set; get; }
        public decimal Total { get; set; }
        public string PaymentMethod { get; set; }
        public OrderStatus Status { set; get; }

        public List<OrderDetail> OrderDetails { get; set; }
        public AppUser AppUser { get; set; }
    }
}
