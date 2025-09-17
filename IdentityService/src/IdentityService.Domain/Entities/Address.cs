using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using IdentityService.Domain.Constants;

namespace IdentityService.Domain.Entities
{
    public class Address : BaseEntity
    {
        [Required]
        [MaxLength(AddressConstants.MaxStreetLength, ErrorMessage = "Street cannot exceed the maximum number of characters.")]
        [MinLength(AddressConstants.MinStreetLength, ErrorMessage = "Street must be at least the minimum number of characters long.")]
        public string Street { get; set; } = string.Empty;

        [Required]
        [MaxLength(AddressConstants.MaxCityLength, ErrorMessage = "City cannot exceed the maximum number of characters.")]
        [MinLength(AddressConstants.MinCityLength, ErrorMessage = "City must be at least the minimum number of characters long.")]
        public string City { get; set; } = string.Empty;

        [Required]
        [MaxLength(AddressConstants.MaxStateLength, ErrorMessage = "State cannot exceed the maximum number of characters.")]
        [MinLength(AddressConstants.MinStateLength, ErrorMessage = "State must be at least the minimum number of characters long.")]
        public string State { get; set; } = string.Empty;

        [Required]
        [MaxLength(AddressConstants.MaxZipCodeLength, ErrorMessage = "ZipCode cannot exceed the maximum number of characters.")]
        [MinLength(AddressConstants.MinZipCodeLength, ErrorMessage = "ZipCode must be at least the minimum number of characters long.")]
        public string ZipCode { get; set; } = string.Empty;

        [Required]
        [MaxLength(AddressConstants.MaxCountryLength, ErrorMessage = "Country cannot exceed the maximum number of characters.")]
        [MinLength(AddressConstants.MinCountryLength, ErrorMessage = "Country must be at least the minimum number of characters long.")]
        public string Country { get; set; } = string.Empty;
    }
}