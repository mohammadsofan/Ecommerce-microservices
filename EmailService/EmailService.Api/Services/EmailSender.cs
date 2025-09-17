using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using EmailService.Api.Interfaces;
using EmailService.Api.Models;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Newtonsoft.Json;

namespace EmailService.Api.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly ILogger<EmailSender> _logger;
        private readonly ITemplateBuilder _templateBuilder;
        private readonly EmailConfiguration _emailConfiguration;
        public EmailSender(
            ILogger<EmailSender> logger,
            ITemplateBuilder templateBuilder,
            IOptions<EmailConfiguration> emailConfiguration
            )
        {
            _logger = logger;
            _templateBuilder = templateBuilder;
            _emailConfiguration = emailConfiguration.Value;
        }
        public async Task SendEmailAsync(EmailFormat email)
        {
            try
            {
                _logger.LogInformation("Try to send email message.");
               
                var body = _templateBuilder.Build(email);
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(MailboxAddress.Parse(_emailConfiguration.From));
                emailMessage.To.Add(MailboxAddress.Parse(email.To));
                emailMessage.Subject = email.Subject;
                emailMessage.Body = new TextPart("html")
                {
                    Text = body
                };

                using var smtp = new MailKit.Net.Smtp.SmtpClient();
                await smtp.ConnectAsync(_emailConfiguration.SmtpServer, _emailConfiguration.Port, SecureSocketOptions.Auto);
                await smtp.AuthenticateAsync(_emailConfiguration.UserName, _emailConfiguration.Password);
                await smtp.SendAsync(emailMessage);
                await smtp.DisconnectAsync(true);
                _logger.LogInformation("Email sent successfully to {To}.", email.To);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email {Subject} for: {Recipient}", email.Subject, email.To);
            }
        }
    }
}