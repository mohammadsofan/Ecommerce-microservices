using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityService.Api.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                throw new InvalidOperationException("User ID claim not found.");
            }

            return userIdClaim.Value;
        }
    }
}