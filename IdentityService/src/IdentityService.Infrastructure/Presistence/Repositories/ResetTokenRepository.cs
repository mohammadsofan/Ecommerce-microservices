using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityService.Application.IRepository;
using IdentityService.Domain.Entities;

namespace IdentityService.Infrastructure.Presistence.Repositories
{
    public class ResetTokenRepository : GenericRepository<ResetToken>,IResetTokenRepository
    {
        public ResetTokenRepository(MongoDbContext context) : base(context)
        {
        }
    }
    
}