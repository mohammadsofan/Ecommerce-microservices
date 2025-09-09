using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using IdentityService.Application.Dtos.Address;
using IdentityService.Domain.Constants;

namespace IdentityService.Application.Validations.Address
{
    public class CreateAddressDtoValidator : AbstractValidator<CreateAddressDto>
    {
        public CreateAddressDtoValidator()
        {
            RuleFor(x => x.Street)
                .NotEmpty().WithMessage("Street is required.")
                .MaximumLength(AddressConstants.MaxStreetLength).WithMessage($"Street cannot exceed {AddressConstants.MaxStreetLength} characters.")
                .MinimumLength(AddressConstants.MinStreetLength).WithMessage($"Street must be at least {AddressConstants.MinStreetLength} characters long.");
            RuleFor(x => x.City)
                .NotEmpty().WithMessage("City is required.")
                .MaximumLength(AddressConstants.MaxCityLength).WithMessage($"City cannot exceed {AddressConstants.MaxCityLength} characters.")
                .MinimumLength(AddressConstants.MinCityLength).WithMessage($"City must be at least {AddressConstants.MinCityLength} characters long.");
            RuleFor(x => x.State)
                .NotEmpty().WithMessage("State is required.")
                .MaximumLength(AddressConstants.MaxStateLength).WithMessage($"State cannot exceed {AddressConstants.MaxStateLength} characters.")
                .MinimumLength(AddressConstants.MinStateLength).WithMessage($"State must be at least {AddressConstants.MinStateLength} characters long.");
            RuleFor(x => x.ZipCode)
                .NotEmpty().WithMessage("ZipCode is required.")
                .MaximumLength(AddressConstants.MaxZipCodeLength).WithMessage($"ZipCode cannot exceed {AddressConstants.MaxZipCodeLength} characters.")
                .MinimumLength(AddressConstants.MinZipCodeLength).WithMessage($"ZipCode must be at least {AddressConstants.MinZipCodeLength} characters long.");
            RuleFor(x => x.Country)
                .NotEmpty().WithMessage("Country is required.")
                .MaximumLength(AddressConstants.MaxCountryLength).WithMessage($"Country cannot exceed {AddressConstants.MaxCountryLength} characters.")
                .MinimumLength(AddressConstants.MinCountryLength).WithMessage($"Country must be at least {AddressConstants.MinCountryLength} characters long.");

        }
        
    }
}