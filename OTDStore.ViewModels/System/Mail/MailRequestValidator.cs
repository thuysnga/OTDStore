using FluentValidation;

namespace OTDStore.ViewModels.System.Mail
{
    public class MailRequestValidator : AbstractValidator<MailRequest>
    {
        public MailRequestValidator()
        {
            RuleFor(x => x.ToEmail).NotEmpty().WithMessage("Nhập email")
                .Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")
                .WithMessage("Không đúng định dạng");
        }
    }
}
