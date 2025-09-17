using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using IdentityService.Application.Dtos.Auth;

namespace IdentityService.Application.Validations.Auth
{
    public class LoginReqDtoValidator : AbstractValidator<LoginReqDto>
    {
        public LoginReqDtoValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.");
        }
    }
}