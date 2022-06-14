using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OTDStore.Utilities.Constants
{
    public class SystemConstants
    {
        public const string MainConnectionString = "OTDStoreDb";
        public const string CartSession = "CartSession";
        public class AppSettings
        {
            public const string Token = "Token";
            public const string BaseAddress = "BaseAddress";
        }

        public class ProductSettings
        {
            public const int NumberOfProducts = 10;
        }
    }
}
