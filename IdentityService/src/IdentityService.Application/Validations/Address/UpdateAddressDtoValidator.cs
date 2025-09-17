using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using IdentityService.Application.Dtos.Address;
using IdentityService.Domain.Constants;

namespace IdentityService.Application.Validations.Address
{
    public class UpdateAddressDtoValidator : AbstractValidator<UpdateAddressDto>
    {
        public UpdateAddressDtoValidator()
        {
            RuleFor(x => x.Street)
               .MaximumLength(AddressConstants.MaxStreetLength).WithMessage($"Street cannot exceed {AddressConstants.MaxStreetLength} characters.")
               .MinimumLength(AddressConstants.MinStreetLength).WithMessage($"Street must be at least {AddressConstants.MinStreetLength} characters long.")
               .When(x => x.Street != null);
            RuleFor(x => x.City)
                .MaximumLength(AddressConstants.MaxCityLength).WithMessage($"City cannot exceed {AddressConstants.MaxCityLength} characters.")
                .MinimumLength(AddressConstants.MinCityLength).WithMessage($"City must be at least {AddressConstants.MinCityLength} characters long.")
                .When(x => x.City != null);
            RuleFor(x => x.State)
                .MaximumLength(AddressConstants.MaxStateLength).WithMessage($"State cannot exceed {AddressConstants.MaxStateLength} characters.")
                .MinimumLength(AddressConstants.MinStateLength).WithMessage($"State must be at least {AddressConstants.MinStateLength} characters long.")
                .When(x => x.State != null);
            RuleFor(x => x.ZipCode)
                .MaximumLength(AddressConstants.MaxZipCodeLength).WithMessage($"ZipCode cannot exceed {AddressConstants.MaxZipCodeLength} characters.")
                .MinimumLength(AddressConstants.MinZipCodeLength).WithMessage($"ZipCode must be at least {AddressConstants.MinZipCodeLength} characters long.")
                .When(x => x.ZipCode != null);
            RuleFor(x => x.Country)
                .MaximumLength(AddressConstants.MaxCountryLength).WithMessage($"Country cannot exceed {AddressConstants.MaxCountryLength} characters.")
                .MinimumLength(AddressConstants.MinCountryLength).WithMessage($"Country must be at least {AddressConstants.MinCountryLength} characters long.")
                .When(x => x.Country != null);

          
           
        }
        
    }
}