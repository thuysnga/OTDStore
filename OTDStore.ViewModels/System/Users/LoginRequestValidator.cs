using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace OTDStore.ViewModels.System.Users
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Vui lòng nhập tài khoản");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Vui lòng nhập mật khẩu")
                .MinimumLength(6).WithMessage("Mật khẩu có ít nhất 6 kí tự");
        }
    }
}
