using OTDStore.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace OTDStore.ViewModels.Catalog.Products
{
    public class GetManageProductPagingRequest : PagingRequestBase
    {
        public string Keyword { get; set; }
        public int? CategoryId { get; set; }
        public int? BrandId { get; set; }
    }
}
