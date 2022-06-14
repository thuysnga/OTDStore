using OTDStore.ViewModels.Common;
using OTDStore.ViewModels.System.Mail;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OTDStore.Application.System.Mail
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
        Task<ApiResult<bool>> ResetPassword(MailRequest mailRequest);
    }
}
