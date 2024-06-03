using FluentValidation;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Features.Authentication.Commands.Models;
using SchoolProject.Core.Resources;

namespace SchoolProject.Core.Features.Authentication.Commands.Validators
{
    public class SIgnInValidator : AbstractValidator<SignInCommand>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region Constructors
        public SIgnInValidator(IStringLocalizer<SharedResources> stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
            ApplyValidationsRules();
            ApplyCustomValidationsRules();
        }
        #endregion

        #region Functions
        public void ApplyValidationsRules()
        {
            RuleFor(s => s.UserName)
               .NotEmpty().WithMessage(_stringLocalizer[SharedResourceKeys.UserName] + _stringLocalizer[SharedResourceKeys.NotEmpty])
               .NotNull().WithMessage(_stringLocalizer[SharedResourceKeys.UserName] + _stringLocalizer[SharedResourceKeys.Required]);

            RuleFor(s => s.Password)
               .NotEmpty().WithMessage(_stringLocalizer[SharedResourceKeys.Password] + _stringLocalizer[SharedResourceKeys.NotEmpty])
               .NotNull().WithMessage(_stringLocalizer[SharedResourceKeys.Password] + _stringLocalizer[SharedResourceKeys.Required]);
        }

        public void ApplyCustomValidationsRules()
        {
        }
        #endregion
    }
}
