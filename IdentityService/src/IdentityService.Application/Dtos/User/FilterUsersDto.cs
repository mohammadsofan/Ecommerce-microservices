using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Application.Dtos.User
{
    public class FilterUsersDto
    {
        public string? Username { get; set; }
        public string? FullName { get; set; }
        public bool? IsActive { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
    }
}