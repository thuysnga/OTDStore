using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OTDStore.Application.System.Mail;
using OTDStore.ViewModels.System.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OTDStore.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailsController : ControllerBase
    {
        private readonly IMailService _mailService;
        public MailsController(IMailService mailService)
        {
            _mailService = mailService;
        }

        //[HttpPost("Send")]
        //public async Task<IActionResult> Send([FromForm] MailRequest request)
        //{
        //    try
        //    {
        //        await mailService.SendEmailAsync(request);
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        [HttpPut]
        public async Task<IActionResult> ResetPassword([FromBody] MailRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mailService.ResetPassword(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}

