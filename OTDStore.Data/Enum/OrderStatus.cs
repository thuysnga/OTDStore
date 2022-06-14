using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OTDStore.Data.Enum
{
    public enum OrderStatus
    {
        [Display(Name = "Đang xử lí")]
        InProgress = 0,
        [Display(Name = "Đã xác nhận")]
        Confirmed = 1,
        [Display(Name = "Đang vận chuyển")]
        Shipping = 2,
        [Display(Name = "Đã nhận hàng")]
        Success = 3,
        [Display(Name = "Đã hủy")]
        Canceled = 4
    }
}
