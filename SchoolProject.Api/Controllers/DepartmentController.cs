using MediatR;
using Microsoft.AspNetCore.Mvc;
using SchoolProject.Api.Base;
using SchoolProject.Core.Features.Department.Queries.Models;
using SchoolProject.Data.AppMetaData;

namespace SchoolProject.Api.Controllers
{
    [ApiController]
    public class DepartmentController : AppControllerBase
    {
        #region Fields
        public readonly IMediator _mediator;
        #endregion

        #region Constructors
        public DepartmentController(IMediator mediator)
        {
            _mediator = mediator;
        }
        #endregion

        #region Handle Fubctions
        [HttpGet(Router.DepartmentRouting.GetByID)]
        public async Task<IActionResult> GetDepartmentByID([FromQuery] GetDepartmentByIdQuery query)
        {
            //var response = await _mediator.Send(new GetStudentByIDQuery() {Id=id}); 
            //or
            return NewResult(await _mediator.Send(query));
        }
        #endregion
    }
}
