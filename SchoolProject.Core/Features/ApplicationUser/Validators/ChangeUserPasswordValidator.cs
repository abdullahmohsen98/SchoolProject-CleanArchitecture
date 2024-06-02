using FluentValidation;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Features.ApplicationUser.Commands.Models;
using SchoolProject.Core.Resources;

namespace SchoolProject.Core.Features.ApplicationUser.Validators
{
    public class ChangeUserPasswordValidator : AbstractValidator<ChangeUserPasswordCommand>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region Constructors
        public ChangeUserPasswordValidator(IStringLocalizer<SharedResources> stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
            ApplyValidationsRules();
            ApplyCustomValidationsRules();
        }
        #endregion

        #region Functions
        public void ApplyValidationsRules()
        {
            RuleFor(s => s.Id)
               .NotEmpty().WithMessage(_stringLocalizer[SharedResourceKeys.Id] + _stringLocalizer[SharedResourceKeys.NotEmpty])
               .NotNull().WithMessage(_stringLocalizer[SharedResourceKeys.Id] + _stringLocalizer[SharedResourceKeys.Required]);

            RuleFor(s => s.CurrentPassword)
               .NotEmpty().WithMessage(_stringLocalizer[SharedResourceKeys.CurrentPassword] + _stringLocalizer[SharedResourceKeys.NotEmpty])
               .NotNull().WithMessage(_stringLocalizer[SharedResourceKeys.CurrentPassword] + _stringLocalizer[SharedResourceKeys.Required]);

            RuleFor(s => s.NewPassword)
               .NotEmpty().WithMessage(_stringLocalizer[SharedResourceKeys.NewPassword] + _stringLocalizer[SharedResourceKeys.NotEmpty])
               .NotNull().WithMessage(_stringLocalizer[SharedResourceKeys.NewPassword] + _stringLocalizer[SharedResourceKeys.Required]);

            RuleFor(s => s.ConfirmPassword)
               .Equal(u => u.NewPassword).WithMessage(_stringLocalizer[SharedResourceKeys.PasswordNotEqualConfirmPassword]);
        }

        public void ApplyCustomValidationsRules()
        {
        }
        #endregion
    }
}
