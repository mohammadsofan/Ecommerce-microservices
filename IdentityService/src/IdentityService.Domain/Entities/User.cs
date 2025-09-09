using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using IdentityService.Domain.Constants;
using IdentityService.Domain.Enums;
namespace IdentityService.Domain.Entities
{
    public class User : BaseEntity
    {
        [Required]
        [MaxLength(UserConstants.MaxUsernameLength, ErrorMessage = "Username cannot exceed the maximum number of characters.")]
        [MinLength(UserConstants.MinUsernameLength, ErrorMessage = "Username must be at least the minimum number of characters long.")]
        public string Username { get; set; } = string.Empty;

        [Required]
        [MaxLength(UserConstants.MaxFullNameLength, ErrorMessage = "Full Name cannot exceed the maximum number of characters.")]
        [MinLength(UserConstants.MinFullNameLength, ErrorMessage = "Full Name must be at least the minimum number of characters long.")]
        public string FullName { get; set; } = string.Empty;
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; } = string.Empty;
        [Phone(ErrorMessage = "Invalid phone number.")]
        public string PhoneNumber { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.User;
        public bool IsActive { get; set; } = true;
        [Length(
            AddressConstants.MinAddressesPerUser, AddressConstants.MaxAddressesPerUser,
            ErrorMessage = "A user must have at least the minimum number of addresses and no more than the maximum number of addresses.")
        ]
        public List<Address> Addresses { get; set; } = new List<Address>();

        public void AddAddress(Address address)
        {
            if (Addresses.Count >= AddressConstants.MaxAddressesPerUser)
            {
                throw new InvalidOperationException($"A user cannot have more than {AddressConstants.MaxAddressesPerUser} addresses.");
            }
            Addresses.Add(address);
        }

       public void RemoveAddress(string addressId)
       {
           if (Addresses.Count <= AddressConstants.MinAddressesPerUser)
           {
               throw new InvalidOperationException("A user must have at least one address.");
           }
           var address = Addresses.FirstOrDefault(a => a.Id == addressId)
            ?? throw new KeyNotFoundException("The specified address was not found for this user.");
            
           Addresses.Remove(address);
       }
     

    }
}