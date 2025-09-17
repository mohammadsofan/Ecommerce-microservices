using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Domain.Constants
{
    public class AddressConstants
    {
        public const int MaxStreetLength = 100;
        public const int MaxCityLength = 100;
        public const int MaxStateLength = 100;
        public const int MaxZipCodeLength = 20;
        public const int MaxCountryLength = 100;
        public const int MinStreetLength = 5;
        public const int MinCityLength = 2;
        public const int MinStateLength = 2;
        public const int MinZipCodeLength = 4;
        public const int MinCountryLength = 2;
        public const int MaxAddressesPerUser = 3;
        public const int MinAddressesPerUser = 1;
        
    }
}