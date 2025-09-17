using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmailService.Api.Models;

namespace EmailService.Api.Interfaces
{
    public interface ITemplateBuilder
    {
        string Build(EmailFormat format);   
    }
}