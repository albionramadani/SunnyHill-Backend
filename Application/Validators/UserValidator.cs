using Backend.Models;
using Domain.Constants;
using FluentValidation;

namespace Application.Validators
{
    public class UserValidator : AbstractValidator<RegisterModel>
    {
        public UserValidator()
        {

            RuleFor(x => x.FirstName).NotNull().NotEmpty();
            RuleFor(x => x.LastName).NotNull().NotEmpty();
            RuleFor(x => x.PhoneNumber).NotNull().NotEmpty();
            RuleFor(x => x.Address).NotNull().NotEmpty();
          
            RuleFor(x => x.Email).NotNull().EmailAddress();
            RuleFor(x => x.Password).NotNull().MinimumLength(Lengths.PasswordMinLength);
            RuleFor(x => x.ConfirmPassword).Matches(x => x.Password);

        }
    }
}
