using FluentValidation;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Features.Authorization.Commands.Models;
using SchoolProject.Core.Resources;

namespace SchoolProject.Core.Features.Authorization.Commands.Validators
{
    internal class EditRoleValidator : AbstractValidator<EditRoleCommand>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region Constructors
        public EditRoleValidator(IStringLocalizer<SharedResources> stringLocalizer)
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

            RuleFor(s => s.Name)
               .NotEmpty().WithMessage(_stringLocalizer[SharedResourceKeys.RoleName] + _stringLocalizer[SharedResourceKeys.NotEmpty])
               .NotNull().WithMessage(_stringLocalizer[SharedResourceKeys.RoleName] + _stringLocalizer[SharedResourceKeys.Required])
               .MaximumLength(100).WithMessage(_stringLocalizer[SharedResourceKeys.RoleName] + _stringLocalizer[SharedResourceKeys.MaxLengthis100]);

        }

        public void ApplyCustomValidationsRules()
        {
        }
        #endregion
    }
}

