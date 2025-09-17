using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Events
{
    public record class ForgotPasswordEvent
    {
        public string Email { get; init; }
        public string OTP { get; init; }
    }
}