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
            var email = $"""
            <html>
                <body class={BaseTemplateStyles.BodyContainer}>
                    <div class={BaseTemplateStyles.EmailContainer}>
                        <div class={BaseTemplateStyles.Header}>
                            {BuildHeader(format.Header)}
                        </div>
                        <div class={BaseTemplateStyles.Content}>
                            {BuildBody(format.Body)}
                        </div>
                        <div class={BaseTemplateStyles.Footer}>
                            {BuildFooter(format.Footer)}
                        </div>
                    </div>
                </body>
            </html>
            """;
            return email;
            
        }
        public  string BuildHeader(string header)
        {
            return $"""
                <h1>
                    {header}
                </h1>
            """;
        }
        public  string BuildBody(string body)
        {
            return $"""
                <p>
                    {body}
                </p>
            """;
        
        }
        public  string BuildFooter(string footer)
        {
            return $"""
                <footer>
                    {footer}
                </footer>
            """;
        }
    }
}