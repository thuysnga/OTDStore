using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OTDStore.ViewModels.System.Users
{
    public class PasswordUpdateRequest
    {
        [Display(Name = "Mật khẩu cũ")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
        [Display(Name = "Mật khẩu mới")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Display(Name = "Xác nhận mật khẩu mới")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
