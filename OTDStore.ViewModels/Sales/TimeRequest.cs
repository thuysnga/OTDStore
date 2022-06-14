using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OTDStore.ViewModels.Sales
{
    public class TimeRequest
    {
        [Display(Name = "Ngày bắt đầu")]
        [DataType(DataType.Date)]
        public DateTime beginT { get; set; }

        [Display(Name = "Ngày kết thúc")]
        [DataType(DataType.Date)]
        public DateTime endT { get; set; }
    }
}
