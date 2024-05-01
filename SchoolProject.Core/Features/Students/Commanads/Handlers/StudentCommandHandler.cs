using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.Students.Commanads.Models;
using SchoolProject.Core.Resources;
using SchoolProject.Data.Entities;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Core.Features.Students.Commanads.Handlers
{
    public class StudentCommandHandler : ResponseHandler,
                                         IRequestHandler<AddStudentCommand, Response<string>>,
                                         IRequestHandler<EditStudentCommand, Response<string>>,
                                         IRequestHandler<DeleteStudentCommand, Response<string>>
    {
        #region Fields
        private readonly IStudentService _studentService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region Constructors
        public StudentCommandHandler(IStudentService studentService,
                                     IMapper mapper,
                                     IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _studentService = studentService;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
        }
        #endregion

        #region Functions
        public async Task<Response<string>> Handle(AddStudentCommand request, CancellationToken cancellationToken)
        {
            // Map AddStudentCommand(request) obj to appropriate model(Student)
            var studentMapper = _mapper.Map<Student>(request);

            // Use studentModel to perform actions(Add)
            var result = await _studentService.AddAsync(studentMapper);

            // Check Conditions
            // Return appropriate response
            if (result == "Success") return Created("");
            else return BadRequest<string>();
        }

        public async Task<Response<string>> Handle(EditStudentCommand request, CancellationToken cancellationToken)
        {
            // Check if the Id is exit or not
            var student = await _studentService.GetByIdAsync(request.Id);
            if (student == null) // return NotFound
                return NotFound<string>("Student is not found");

            // Map EditStudentCommand(request) obj to appropriate model(Student)\
            var studentMapper = _mapper.Map(request, student);

            // Call service that make edit
            var result = await _studentService.EditAsync(studentMapper);

            // Return appropriate response
            if (result == "Success") return Success<string>($"{studentMapper.StudID} Edited Successfully");
            else return BadRequest<string>();
        }

        public async Task<Response<string>> Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
        {
            // Check if the Id is exit or not
            var student = await _studentService.GetByIdAsync(request.Id);
            if (student == null) // return NotFound
                return NotFound<string>("Student is not found");

            // Call service that make edit
            var result = await _studentService.DeleteAsync(student);

            // Return appropriate response
            if (result == "Success") return Deleted<string>($"{request.Id} Deleted Successfully");
            else return BadRequest<string>();
        }
        #endregion
    }
}

