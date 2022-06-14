using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using OTDStore.Data.Entities;
using OTDStore.ViewModels.Common;
using OTDStore.ViewModels.System.Mail;
using System.Threading.Tasks;
using System.Text;

namespace OTDStore.Application.System.Mail
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _config;

        public MailService(IOptions<MailSettings> mailSettings, IConfiguration config,
                            UserManager<AppUser> userManager)
        {
            _mailSettings = mailSettings.Value;
            _userManager = userManager;
            _config = config;
        }
        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;
            var builder = new BodyBuilder();
            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }

        public async Task<ApiResult<bool>> ResetPassword(MailRequest mailRequest)
        {
            var user = await _userManager.FindByEmailAsync(mailRequest.ToEmail);
            if (user == null) return new ApiErrorResult<bool>("Email không tồn tại");
            var newpassword = "Password@123";
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = "Reset Password";
            var builder = new BodyBuilder();

            var tmp = await _userManager.RemovePasswordAsync(user);
            var result = await _userManager.AddPasswordAsync(user, newpassword);
            
            builder.HtmlBody = $"New password: {newpassword}. Please change your password!!!";
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
            return new ApiSuccessResult<bool>();
        }
    }
}
