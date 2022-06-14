using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace OTDStore.ViewModels.Sales
{
    public class CheckoutRequestValidator : AbstractValidator<CheckoutRequest>
    {
        public CheckoutRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Nhập tên người nhận hàng");

            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Nhập số điện thoại")
                .MaximumLength(11).WithMessage("Số điện thoại không hợp lệ")
                .MinimumLength(10).WithMessage("Số điện thoại không hợp lệ");

            RuleFor(x => x.Address).NotEmpty().WithMessage("Nhập địa chỉ nhận hàng");

            RuleFor(x => x.Email).NotEmpty().WithMessage("Nhập email nhận hàng")
                .Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")
                .WithMessage("Không đúng định dạng");

            RuleFor(x => x.PaymentMethod).NotEmpty().WithMessage("Vui lòng chọn phương thức thanh toán");
        }
    }
}
