using FluentValidation;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Features.Students.Commanads.Models;
using SchoolProject.Core.Resources;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Core.Features.Students.Commanads.Validators
{
    public class EditStudentValidator : AbstractValidator<EditStudentCommand>
    {
        #region Fields
        private readonly IStudentService _studentService;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region Constructors
        public EditStudentValidator(IStudentService studentService,
                                   IStringLocalizer<SharedResources> stringLocalizer)
        {
            _studentService = studentService;
            _stringLocalizer = stringLocalizer;
            ApplyValidationsRules();
            ApplyCustomValidationsRules();
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
                .MustAsync(async (model, Key, CancellationToken) => !await _studentService.IsNameEnExistExcludeSelf(Key, model.Id))
                .WithMessage(_stringLocalizer[SharedResourceKeys.Name] + _stringLocalizer[SharedResourceKeys.IsExist]);

            RuleFor(s => s.NameEn)
                .MustAsync(async (model, Key, CancellationToken) => !await _studentService.IsNameEnExistExcludeSelf(Key, model.Id))
                .WithMessage(_stringLocalizer[SharedResourceKeys.Name] + _stringLocalizer[SharedResourceKeys.IsExist]);
        }
        #endregion
    }
}
