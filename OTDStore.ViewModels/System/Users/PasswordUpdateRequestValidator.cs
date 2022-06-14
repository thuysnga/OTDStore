using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace OTDStore.ViewModels.System.Users
{
    public class PasswordUpdateRequestValidator : AbstractValidator<PasswordUpdateRequest>
    {
        public PasswordUpdateRequestValidator()
        {
            RuleFor(x => x.NewPassword).NotEmpty().WithMessage("Nhập mật khẩu")
                .MinimumLength(6).WithMessage("Mật khẩu có ít nhất 6 kí tự");

            RuleFor(x => x).Custom((request, context) =>
            {
                if (request.NewPassword != request.ConfirmPassword)
                {
                    context.AddFailure("Xác nhận mật khẩu không đúng");
                }
            });
        }
    }
}
