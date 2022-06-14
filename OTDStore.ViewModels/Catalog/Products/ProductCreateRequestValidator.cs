using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace OTDStore.ViewModels.Catalog.Products
{
    public class ProductCreateRequestValidator : AbstractValidator<ProductCreateRequest>
    {
        public ProductCreateRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Nhập tên sản phẩm")
                .MaximumLength(200).WithMessage("Tên không vượt quá 200 kí tự");

            RuleFor(x => x.Description).NotEmpty().WithMessage("Nhập mô tả sản phẩm");

            RuleFor(x => x.YearRelease).NotEmpty().WithMessage("Nhập năm ra mắt")
                .MaximumLength(4).WithMessage("Năm không vượt quá 4 kí tự");

            RuleFor(x => x.Insurance).NotEmpty().WithMessage("Nhập thời gian bảo hành")
                .MaximumLength(200);

            RuleFor(x => x.Color).NotEmpty().WithMessage("Nhập màu sắc")
                .MaximumLength(200).WithMessage("Không vượt quá 200 kí tự");

            RuleFor(x => x.CPU).NotEmpty().WithMessage("Nhập CPU")
                .MaximumLength(200).WithMessage("Không vượt quá 200 kí tự");

            RuleFor(x => x.Memory).NotEmpty().WithMessage("Nhập dung lượng bộ nhớ")
                .MaximumLength(200).WithMessage("Không vượt quá 200 kí tự");

            RuleFor(x => x.RAM).NotEmpty().WithMessage("Nhập dung lượng RAM")
                .MaximumLength(200).WithMessage("Không vượt quá 200 kí tự");

            RuleFor(x => x.Camera).NotEmpty().WithMessage("Nhập thông số camera")
                .MaximumLength(200);

            RuleFor(x => x.Bluetooth).NotEmpty().WithMessage("Nhập thông số bluetooth")
                .MaximumLength(200);

            RuleFor(x => x.Monitor).NotEmpty().WithMessage("Nhập thông số màn hình")
                .MaximumLength(100);

            RuleFor(x => x.Battery).NotEmpty().WithMessage("Nhập thông số pin")
                .MaximumLength(50);

            RuleFor(x => x.Size).NotEmpty().WithMessage("Nhập kích thước")
                .MaximumLength(50).WithMessage("Không vượt quá 50 kí tự");

            RuleFor(x => x.OS).NotEmpty().WithMessage("Nhập OS")
                .MaximumLength(50).WithMessage("Không vượt quá 50 kí tự");

            RuleFor(x => x.Price).NotEmpty().WithMessage("Nhập giá bán");

            RuleFor(x => x.OriginalPrice).NotEmpty().WithMessage("Nhập giá nhập kho");

            RuleFor(x => x.Stock).NotEmpty().WithMessage("Nhập số lượng");

            RuleFor(x => x.ThumbnailImage).NotEmpty().WithMessage("Chọn ảnh đại diện");
        }
    }
}
