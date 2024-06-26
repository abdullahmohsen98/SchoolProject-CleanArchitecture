using FluentValidation;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Features.Authorization.Commands.Models;
using SchoolProject.Core.Resources;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Core.Features.Authorization.Commands.Validators
{
    public class AddRoleValidator : AbstractValidator<AddRoleCommand>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        private readonly IAuthorizationSevice _authorizationSevice;
        #endregion

        #region Constructors
        public AddRoleValidator(IStringLocalizer<SharedResources> stringLocalizer,
                                 IAuthorizationSevice authorizationSevice)
        {
            _stringLocalizer = stringLocalizer;
            _authorizationSevice = authorizationSevice;
            ApplyValidationsRules();
            ApplyCustomValidationsRules();
        }
        #endregion

        #region Actions
        public void ApplyValidationsRules()
        {
            RuleFor(s => s.RoleName)
               .NotEmpty().WithMessage(_stringLocalizer[SharedResourceKeys.RoleName] + _stringLocalizer[SharedResourceKeys.NotEmpty])
               .NotNull().WithMessage(_stringLocalizer[SharedResourceKeys.RoleName] + _stringLocalizer[SharedResourceKeys.Required]);
        }

        public void ApplyCustomValidationsRules()
        {
            RuleFor(s => s.RoleName)
                .MustAsync(async (roleName, CancellationToken) => !await _authorizationSevice.IsRoleExistByName(roleName))
                .WithMessage(_stringLocalizer[SharedResourceKeys.Role] + _stringLocalizer[SharedResourceKeys.IsExist]);
        }
        #endregion
    }
}
