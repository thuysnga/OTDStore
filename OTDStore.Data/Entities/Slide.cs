using OTDStore.Data.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace OTDStore.Data.Entities
{
    public class Slide
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public string Description { get; set; }
        public string Url { get; set; }
        public int SortOrder { set; get; }
        public string Image { get; set; }
        public Status Status { set; get; }
    }
}
