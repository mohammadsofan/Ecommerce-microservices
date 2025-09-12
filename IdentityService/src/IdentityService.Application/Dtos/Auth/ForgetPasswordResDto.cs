using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Application.Dtos.Auth
{
    public class ForgetPasswordResDto
    {
        public string OtpId { get; set; } = string.Empty;
    }
}