using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Application.Dtos.Auth
{
    public class ForgetPasswordReqDto
    {
        public string Email { get; set; } = string.Empty;
    }
}