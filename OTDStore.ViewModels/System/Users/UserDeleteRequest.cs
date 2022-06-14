using System;
using System.Collections.Generic;
using System.Text;

namespace OTDStore.ViewModels.System.Users
{
    public class UserDeleteRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
