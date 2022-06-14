﻿using OTDStore.Data.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace OTDStore.Data.Entities
{
    public class Category
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public int SortOrder { set; get; }
        public bool IsShowOnHome { set; get; }
        public int? ParentId { set; get; }
        public Status Status { set; get; }

        public List<ProductInCategory> ProductInCategories { get; set; }
    }
}
