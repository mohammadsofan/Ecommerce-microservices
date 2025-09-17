using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Domain.Entities
{
    public class ResetToken : BaseEntity
    {
        public string UserId { get; set; } = string.Empty;
        public DateTime ExpiryTime { get; set; }
    }
}