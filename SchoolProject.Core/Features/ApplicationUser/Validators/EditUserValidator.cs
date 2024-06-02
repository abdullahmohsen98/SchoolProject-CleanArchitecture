using FluentValidation;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Features.ApplicationUser.Commands.Models;
using SchoolProject.Core.Resources;

namespace SchoolProject.Core.Features.ApplicationUser.Validators
{
    public class EditUserValidator : AbstractValidator<EditUserCommand>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region Constructors
        public EditUserValidator(IStringLocalizer<SharedResources> stringLocalizer)
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
        }

        public void ApplyCustomValidationsRules()
        {
        }
        #endregion
    }
}
