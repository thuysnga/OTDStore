using System;
using System.Collections.Generic;
using System.Text;

namespace OTDStore.ViewModels.Sales
{
    public class OrderDetailVM
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public string Memory { get; set; }
        public string RAM { get; set; }
        public decimal Price { get; set; }
    }
}
