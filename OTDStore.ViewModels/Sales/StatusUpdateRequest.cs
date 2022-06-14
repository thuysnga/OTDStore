using OTDStore.Data.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace OTDStore.ViewModels.Sales
{
    public class StatusUpdateRequest
    {
        public int Id { get; set; }
        public OrderStatus Status { get; set; }
    }
}
