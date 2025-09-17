using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using IdentityService.Application.Dtos.User;
using IdentityService.Domain.Constants;

namespace IdentityService.Application.Validations.User
{
    public class UpdateUserPersonalInfoDtoValidator : AbstractValidator<UpdateUserPersonalInfoDto>
    {
        public UpdateUserPersonalInfoDtoValidator()
        {
            RuleFor(x => x.FullName)
                .MaximumLength(UserConstants.MaxFullNameLength).WithMessage($"Full Name cannot exceed {UserConstants.MaxFullNameLength} characters.")
                .MinimumLength(UserConstants.MinFullNameLength).WithMessage($"Full Name must be at least {UserConstants.MinFullNameLength} characters long.")
                .When(x => x.FullName != null);

            RuleFor(x => x.PhoneNumber)
                .Matches(@"^\+?\d{1,14}(-\d{1,14})*$").WithMessage("Invalid phone number format.")
                .When(x => x.PhoneNumber != null);
        }
        
    }
}