using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace OTDStore.ViewModels.System.Users
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("Nhập tên")
                .MaximumLength(200).WithMessage("First name can not over 200 characters");

            RuleFor(x => x.LastName).NotEmpty().WithMessage("Nhập họ")
                .MaximumLength(200).WithMessage("Last name can not over 200 characters");

            RuleFor(x => x.Dob).GreaterThan(DateTime.Now.AddYears(-100)).WithMessage("Sinh nhật không hợp lệ");

            RuleFor(x => x.Address).NotEmpty().WithMessage("Nhập địa chỉ");

            RuleFor(x => x.Email).NotEmpty().WithMessage("Nhập email")
                .Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")
                .WithMessage("Không đúng định dạng");

            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Nhập số điện thoại")
                .MaximumLength(11).WithMessage("Số điện thoại không hợp lệ")
                .MinimumLength(10).WithMessage("Số điện thoại không hợp lệ");

            RuleFor(x => x.UserName).NotEmpty().WithMessage("Nhập tài khoản");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Nhập mật khẩu")
                .MinimumLength(6).WithMessage("Mật khẩu có ít nhất 6 kí tự");

            RuleFor(x => x).Custom((request, context) =>
            {
                if (request.Password != request.ConfirmPassword)
                {
                    context.AddFailure("Xác nhận mật khẩu không đúng");
                }
            });
        }
    }
}
