using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Application.Dtos.Auth
{
    public class VerifyOtpReqDto
    {
        public string Otp { get; set; } = string.Empty;
        public string OtpId { get; set; } = string.Empty;
    }
}