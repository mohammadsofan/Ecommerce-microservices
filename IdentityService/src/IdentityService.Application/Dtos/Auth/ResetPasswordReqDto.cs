using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Application.Dtos.Auth
{
    public class ResetPasswordReqDto
    {
        public string NewPassword { get; set; } = string.Empty;
        public string ResetTokenId { get; set; } = string.Empty;
    }
}