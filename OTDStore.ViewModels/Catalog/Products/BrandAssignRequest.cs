using OTDStore.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace OTDStore.ViewModels.Catalog.Products
{
     public class BrandAssignRequest
    {
        public int Id { get; set; }
        public List<SelectItem> Brands { get; set; } = new List<SelectItem>();
    }
}
