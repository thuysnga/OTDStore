using System;
using System.Collections.Generic;
using System.Text;

namespace OTDStore.Utilities.Exceptions
{
    public class OTDStoreException : Exception
    {
        public OTDStoreException()
        {
        }

        public OTDStoreException(string message)
            : base(message)
        {
        }

        public OTDStoreException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
