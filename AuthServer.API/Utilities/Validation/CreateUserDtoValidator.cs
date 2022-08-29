using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthServer.Core.DTO;
using FluentValidation;
namespace AuthServer.Shared.Utilities.Validation
{
    public class CreateUserDtoValidator:AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required!")
            .EmailAddress().WithMessage("Please enter a valid Email address!");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required!");

            RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName is required!");
        }
    }
}