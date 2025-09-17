using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityService.Domain.Entities;

namespace IdentityService.Infrastructure.Presistence.Data
{
    public class AddressDataGenerator
    {
        public List<Address> GenerateAddresses()
        {
            var addresses = new List<Address>
            {
                new Address
                {
                    Id = Guid.NewGuid().ToString(),
                    Street = "123 Main St",
                    City = "Springfield",
                    State = "IL",
                    ZipCode = "62701",
                    Country = "USA"
                },
                new Address
                {
                    Id = Guid.NewGuid().ToString(),
                    Street = "456 Elm St",
                    City = "Metropolis",
                    State = "NY",
                    ZipCode = "10001",
                    Country = "USA"
                }
            };

            return addresses;
        }
    }
}