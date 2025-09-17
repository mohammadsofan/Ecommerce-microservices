using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityService.Domain.Entities;

namespace IdentityService.Application.IRepository
{
    public interface IPasswordResetOtpRepository : IGenericRepository<PasswordResetOtp>
    {
        
    }
}