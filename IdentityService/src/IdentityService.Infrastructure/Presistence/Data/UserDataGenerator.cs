using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityService.Application.IAuth;
using IdentityService.Domain.Entities;
using IdentityService.Domain.Enums;

namespace IdentityService.Infrastructure.Presistence.Data
{
    public class UserDataGenerator
    {
        private readonly string _password = "$2a$11$b.0eGeZIhk2Txnpl6YrYl.8TS7S8kcXt9x9AFktVgWRe7cmYt.92C"; // Abc@123456
        private readonly List<Address> _addresses;
        public UserDataGenerator(List<Address> addresses)
        {
            _addresses = addresses;
        }
        public List<User> GenerateUsers()
        {
            var users = new List<User>
           {
               new User
               {
                   Id = Guid.NewGuid().ToString(),
                   Username = "admin_user",
                   FullName = "Admin Doe",
                   Email = "admin.doe@example.com",
                   PasswordHash = _password,
                   Role = UserRole.Admin,
                   PhoneNumber = "123-456-7890",
                   Addresses = new List<Address> { _addresses[0] }
               },
               new User
               {
                   Id = Guid.NewGuid().ToString(),
                   Username = "jane_doe",
                   FullName = "Jane Doe",
                   Email = "jane.doe@example.com",
                   PasswordHash = _password,
                   Role = UserRole.User,
                   PhoneNumber = "123-456-7890",
                   Addresses = new List<Address> { _addresses[1] }
               }
           };
            return users;

        }
    }
}