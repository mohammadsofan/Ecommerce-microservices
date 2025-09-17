using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Application.Exceptions
{
    public class InactiveUserException : AppException
    {
        public InactiveUserException(string userId)
            : base($"User with ID: {userId} is inactive.")
        {
        }
    }
}