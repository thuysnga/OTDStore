using OTDStore.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace OTDStore.ViewModels.Sales
{
    public class GetOrderPagingRequest : PagingRequestBase
    {
        public string Keyword { get; set; }
    }
}
