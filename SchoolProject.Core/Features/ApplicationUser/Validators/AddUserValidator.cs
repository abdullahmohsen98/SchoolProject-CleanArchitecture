using FluentValidation;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Features.ApplicationUser.Commands.Models;
using SchoolProject.Core.Resources;

namespace SchoolProject.Core.Features.ApplicationUser.Validators
{
    public class AddUserValidator : AbstractValidator<AddUserCommand>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region Constructors
        public AddUserValidator(IStringLocalizer<SharedResources> stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
            ApplyValidationsRules();
            ApplyCustomValidationsRules();
        }
        #endregion

        #region Functions
        public void ApplyValidationsRules()
        {
            RuleFor(s => s.FullName)
               .NotEmpty().WithMessage(_stringLocalizer[SharedResourceKeys.FullName] + _stringLocalizer[SharedResourceKeys.NotEmpty])
               .NotNull().WithMessage(_stringLocalizer[SharedResourceKeys.FullName] + _stringLocalizer[SharedResourceKeys.Required])
               .MaximumLength(100).WithMessage(_stringLocalizer[SharedResourceKeys.FullName] + _stringLocalizer[SharedResourceKeys.MaxLengthis100]);

            RuleFor(s => s.UserName)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourceKeys.UserName] + _stringLocalizer[SharedResourceKeys.NotEmpty])
                .NotNull().WithMessage(_stringLocalizer[SharedResourceKeys.Required])
                .MaximumLength(100).WithMessage(_stringLocalizer[SharedResourceKeys.MaxLengthis100]);

            RuleFor(s => s.Email)
               .NotEmpty().WithMessage(_stringLocalizer[SharedResourceKeys.Email] + _stringLocalizer[SharedResourceKeys.NotEmpty])
               .NotNull().WithMessage(_stringLocalizer[SharedResourceKeys.Email] + _stringLocalizer[SharedResourceKeys.Required]);

            RuleFor(s => s.Password)
               .NotEmpty().WithMessage(_stringLocalizer[SharedResourceKeys.Password] + _stringLocalizer[SharedResourceKeys.NotEmpty])
               .NotNull().WithMessage(_stringLocalizer[SharedResourceKeys.Password] + _stringLocalizer[SharedResourceKeys.Required]);

            RuleFor(s => s.ConfirmPassword)
               .Equal(u => u.Password).WithMessage(_stringLocalizer[SharedResourceKeys.ConfirmPassword] + _stringLocalizer[SharedResourceKeys.PasswordNotEqualConfirmPassword]);

        }

        public void ApplyCustomValidationsRules()
        {
        }
        #endregion
    }
}
