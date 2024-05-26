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
        private readonly IDepartmentService _departmentService;
        #endregion

        #region Constructors
        public AddStudentValidator(IStudentService studentService,
                                   IStringLocalizer<SharedResources> stringLocalizer,
                                   IDepartmentService departmentService)
        {
            ApplyValidationsRules();
            ApplyCustomValidationsRules();
            _studentService = studentService;
            _stringLocalizer = stringLocalizer;
            _departmentService = departmentService;
        }
        #endregion

        #region Actions
        public void ApplyValidationsRules()
        {
            RuleFor(s => s.Address)
               .NotEmpty().WithMessage(_stringLocalizer[SharedResourceKeys.Address] + _stringLocalizer[SharedResourceKeys.NotEmpty])
               .NotNull().WithMessage(_stringLocalizer[SharedResourceKeys.Address] + _stringLocalizer[SharedResourceKeys.Required])
               .MaximumLength(100).WithMessage(_stringLocalizer[SharedResourceKeys.Address] + _stringLocalizer[SharedResourceKeys.MaxLengthis100]);

            RuleFor(s => s.NameAr)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourceKeys.Name] + _stringLocalizer[SharedResourceKeys.NotEmpty])
                .NotNull().WithMessage(_stringLocalizer[SharedResourceKeys.Required])
                .MaximumLength(100).WithMessage(_stringLocalizer[SharedResourceKeys.MaxLengthis100]);

            RuleFor(s => s.DepartmentID)
               .NotEmpty().WithMessage(_stringLocalizer[SharedResourceKeys.DepartmentID] + _stringLocalizer[SharedResourceKeys.NotEmpty])
               .NotNull().WithMessage(_stringLocalizer[SharedResourceKeys.DepartmentID] + _stringLocalizer[SharedResourceKeys.Required]);

        }

        public void ApplyCustomValidationsRules()
        {
            RuleFor(s => s.NameAr)
                .MustAsync(async (Key, CancellationToken) => !await _studentService.IsNameArExist(Key))
                .WithMessage(_stringLocalizer[SharedResourceKeys.Name] + _stringLocalizer[SharedResourceKeys.IsExist]);

            RuleFor(s => s.NameEn)
                .MustAsync(async (Key, CancellationToken) => !await _studentService.IsNameEnExist(Key))
                .WithMessage(_stringLocalizer[SharedResourceKeys.Name] + _stringLocalizer[SharedResourceKeys.IsExist]);

            RuleFor(s => s.DepartmentID)
                .MustAsync(async (Key, CancellationToken) => await _departmentService.IsDepartmentIdExist(Key))
                .WithMessage(_stringLocalizer[SharedResourceKeys.IsNotExist]);
        }
        #endregion
    }
}
