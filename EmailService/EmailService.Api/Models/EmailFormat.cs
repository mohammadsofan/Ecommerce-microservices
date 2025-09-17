using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailService.Api.Models
{
    public class EmailFormat
    {
        public string To { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Header { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string Footer { get; set; } = string.Empty;
    }
}