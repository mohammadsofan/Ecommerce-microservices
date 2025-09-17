using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmailService.Api.Interfaces;
using EmailService.Api.Models;
using EmailService.Api.Styles;

namespace EmailService.Api.Templates
{
    public class BaseTemplateBuilder : ITemplateBuilder
    {
        public string Build(EmailFormat format)
        {
            var styles = $"""
                <style>
                {BaseTemplateStyles.BodyContainer}
                {BaseTemplateStyles.EmailContainer}
                {BaseTemplateStyles.Header}
                {BaseTemplateStyles.Content}
                {BaseTemplateStyles.Footer}
                </style>
            """;

            var email = $"""
            <html>
                <head>
                    {styles}
                </head>
                <body class="body-container">
                    <div class="email-container">
                        <div class="header">
                            {BuildHeader(format.Header)}
                        </div>
                        <div class="content">
                            {BuildBody(format.Body)}
                        </div>
                        <div class="footer">
                            {BuildFooter(format.Footer)}
                        </div>
                    </div>
                </body>
            </html>
            """;
            return email;
        }

        public string BuildHeader(string header)
        {
            return $"""
                {header}
            """;
        }
        public string BuildBody(string body)
        {
            return $"""
                {body}
            """;
        }
        public string BuildFooter(string footer)
        {
            return $"""
                {footer}
            """;
        }
    }
}