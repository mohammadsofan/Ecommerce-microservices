using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailService.Api.Styles
{
    public class BaseTemplateStyles
    {
        public const string BodyContainer = """
            .body-container{
                font-family: Arial, sans-serif;
                margin: 0;
                padding: 0;
                background-color: #f4f4f4;
            }
        """;
        public const string EmailContainer = """
            .email-container{
                max-width: 600px;
                margin: 20px auto;
                background: #ffffff;
                border-radius: 8px;
                overflow: hidden;
                box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
            }
        """;
        public const string Header = """
            .header{
                background-color: #007BFF;
                color: #ffffff;
                text-align: center;
                padding: 20px;
            }

        """;
        public const string Content = """
            .content {
                padding: 20px;
                color: #333333;
                line-height: 1.6;
            }
        """;
         public const string Footer = """
            .footer{
                text-align: center;
                padding: 10px;
                font-size: 12px;
                color: #777777;
                background-color: #f4f4f4;
            }

        """;
    }
}