using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityService.Application.IRepository;
using IdentityService.Domain.Entities;

namespace IdentityService.Infrastructure.Presistence.Repositories
{
    public class PasswordResetOtpRepository : GenericRepository<PasswordResetOtp>,IPasswordResetOtpRepository
    {
        public PasswordResetOtpRepository(MongoDbContext context) : base(context)
        {
        }
    }
}