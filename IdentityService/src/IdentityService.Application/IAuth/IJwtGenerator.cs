using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityService.Domain.Entities;
namespace IdentityService.Application.IAuth
{
    public interface IJwtGenerator
    {
        string GenerateJwtToken(User user);
        
    }
}