using FluentValidation;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Features.Students.Commanads.Models;
using SchoolProject.Core.Resources;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Core.Features.Students.Commanads.Validators
{
    public class AddStudentValidator : AbstractValidator<AddStudentCommand>
    {
        #region Fields
        private readonly IStudentService _studentService;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region Constructors
        public AddStudentValidator(IStudentService studentService,
                                   IStringLocalizer<SharedResources> stringLocalizer)
        {
            ApplyValidationsRules();
            ApplyCustomValidationsRules();
            _studentService = studentService;
            _stringLocalizer = stringLocalizer;
        }
        #endregion

        #region Actions
        public void ApplyValidationsRules()
        {
            RuleFor(s => s.NameAr)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourceKeys.Name] + _stringLocalizer[SharedResourceKeys.NotEmpty])
                .NotNull().WithMessage(_stringLocalizer[SharedResourceKeys.Required])
                .MaximumLength(10).WithMessage(_stringLocalizer[SharedResourceKeys.MaxLengthis100]);

            RuleFor(s => s.Address)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourceKeys.Address] + _stringLocalizer[SharedResourceKeys.NotEmpty])
                .NotNull().WithMessage(_stringLocalizer[SharedResourceKeys.Address] + _stringLocalizer[SharedResourceKeys.Required])
                .MaximumLength(10).WithMessage(_stringLocalizer[SharedResourceKeys.Address] + _stringLocalizer[SharedResourceKeys.MaxLengthis100]);
        }

        public void ApplyCustomValidationsRules()
        {
            RuleFor(s => s.NameAr)
                .MustAsync(async (Key, CancellationToken) => !await _studentService.IsNameArExist(Key))
                .WithMessage(_stringLocalizer[SharedResourceKeys.Name] + _stringLocalizer[SharedResourceKeys.IsExist]);

            RuleFor(s => s.NameEn)
                .MustAsync(async (Key, CancellationToken) => !await _studentService.IsNameEnExist(Key))
                .WithMessage(_stringLocalizer[SharedResourceKeys.Name] + _stringLocalizer[SharedResourceKeys.IsExist]);
        }
        #endregion
    }
}
