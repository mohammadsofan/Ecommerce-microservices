using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Application.Exceptions
{
    public class NotFoundException : AppException
    {
        public NotFoundException(string Id, Object? key = null) 
        : base($"Entity \"{Id}\" ({key}) was not found.")
        {
        }
        
    }
}