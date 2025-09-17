using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmailService.Api.Models;

namespace EmailService.Api.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(EmailFormat emailFormat);
    }
}