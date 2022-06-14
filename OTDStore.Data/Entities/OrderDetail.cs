using System;
using System.Collections.Generic;
using System.Text;

namespace OTDStore.Data.Entities
{
    public class OrderDetail
    {
        public int OrderId { set; get; }
        public int ProductId { set; get; }
        public int Quantity { set; get; }
        public string Name { get; set; }
        public string Color { get; set; }
        public string Memory { get; set; }
        public string RAM { get; set; }
        public decimal Price { get; set; }
        public Order Order { get; set; }
        public Product Product { get; set; }

    }
}
