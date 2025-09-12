using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmailService.Api.Interfaces;
using EmailService.Api.Models;
using MassTransit;
using Shared.Events;

namespace EmailService.Api.Consumers
{
    public class ForgotPasswordConsumer : IConsumer<ForgotPasswordEvent>
    {
        private readonly ILogger<ForgotPasswordConsumer> _logger;
        private readonly IEmailSender _emailSender;
        public ForgotPasswordConsumer(
            ILogger<ForgotPasswordConsumer> logger,
            IEmailSender emailSender)
        {
            _logger = logger;
            _emailSender = emailSender;
        }
        public async Task Consume(ConsumeContext<ForgotPasswordEvent> context)
        {
            try
            {
                _logger.LogInformation("Processing ForgotPasswordEvent for: {Email}", context.Message.Email);
                
                var emailFormat=new EmailFormat
                {
                    To = context.Message.Email,
                    Subject = "Password Reset Request",
                    Header = "Password Reset",
                    Body =  $"Your OTP for password reset is: {context.Message.OTP}, please use this to reset your password.",
                    Footer = "If you did not request a password reset, please ignore this email."
                };

                await _emailSender.SendEmailAsync(emailFormat);
                _logger.LogInformation("ForgotPasswordEvent processed successfully for: {Email}", context.Message.Email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing ForgotPasswordEvent for: {Email}", context.Message.Email);
            }

        }
    }
}