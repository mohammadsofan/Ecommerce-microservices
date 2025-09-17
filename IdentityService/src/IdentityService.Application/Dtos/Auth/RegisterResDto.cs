using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityService.Application.Dtos.Address;

namespace IdentityService.Application.Dtos.Auth
{
    public class RegisterResDto
    {
        public string Id { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;   
        public List<AddressDto> Addresses { get; set; } = new List<AddressDto>();  
    }
}