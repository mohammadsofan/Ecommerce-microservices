using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Application.Dtos.Auth
{
    public class VerifyOtpResDto
    {
        public bool IsVerified { get; set; }
        public string ResetToken { get; set; } = string.Empty;
    }
}