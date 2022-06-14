using System;
using System.Collections.Generic;
using System.Text;

namespace OTDStore.ViewModels.Common
{
    public class PagedResult<T> : PagedResultBase
    {
        public List<T> Items { get; set; }
    }
}
