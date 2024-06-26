using FluentValidation;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Features.Authorization.Commands.Models;
using SchoolProject.Core.Resources;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Core.Features.Authorization.Commands.Validators
{
    public class DeleteRoleValidator : AbstractValidator<DeleteRoleCommand>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        private readonly IAuthorizationSevice _authorizationSevice;
        #endregion

        #region Constructors
        public DeleteRoleValidator(IStringLocalizer<SharedResources> stringLocalizer,
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
            RuleFor(s => s.Id)
               .NotEmpty().WithMessage(_stringLocalizer[SharedResourceKeys.NotEmpty])
               .NotNull().WithMessage(_stringLocalizer[SharedResourceKeys.Required]);
        }

        public void ApplyCustomValidationsRules()
        {
            //RuleFor(s => s.Id)
            //    .MustAsync(async (roleId, CancellationToken) => await _authorizationSevice.IsRoleExistById(roleId))
            //    .WithMessage(_stringLocalizer[SharedResourceKeys.Role] + _stringLocalizer[SharedResourceKeys.IsNotExist]);
        }
        #endregion
    }
}
