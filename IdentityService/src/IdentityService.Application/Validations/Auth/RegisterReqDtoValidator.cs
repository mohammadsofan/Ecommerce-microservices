using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using IdentityService.Application.Dtos.Auth;
using IdentityService.Application.Validations.Address;
using IdentityService.Domain.Constants;

namespace IdentityService.Application.Validations.Auth
{
    public class RegisterReqDtoValidator : AbstractValidator<RegisterReqDto>
    {
        public RegisterReqDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Confirm Password is required.")
                .Equal(x => x.Password).WithMessage("Passwords do not match.");
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required.")
                .MinimumLength(UserConstants.MinUsernameLength).WithMessage($"Username must be at least {UserConstants.MinUsernameLength} characters long.")
                .MaximumLength(UserConstants.MaxUsernameLength).WithMessage($"Username cannot exceed {UserConstants.MaxUsernameLength} characters.");
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full Name is required.")
                .MaximumLength(UserConstants.MaxFullNameLength).WithMessage($"Full Name cannot exceed {UserConstants.MaxFullNameLength} characters.")
                .MinimumLength(UserConstants.MinFullNameLength).WithMessage($"Full Name must be at least {UserConstants.MinFullNameLength} characters long.");
                RuleFor(x => x.PhoneNumber)
                    .NotEmpty().WithMessage("Phone Number is required.")
                    .Matches(@"^\+?\d{1,14}(-\d{1,14})*$").WithMessage("Invalid phone number format.");
            RuleFor(x => x.Address)
                .SetValidator(new CreateAddressDtoValidator());
        }
    }
}