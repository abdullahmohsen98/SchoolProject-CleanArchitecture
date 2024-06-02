using MediatR;
using Microsoft.AspNetCore.Mvc;
using SchoolProject.Api.Base;
using SchoolProject.Core.Features.ApplicationUser.Commands.Models;
using SchoolProject.Core.Features.ApplicationUser.Queries.Modles;
using SchoolProject.Data.AppMetaData;

namespace SchoolProject.Api.Controllers
{
    [ApiController]
    public class ApplicationUserController : AppControllerBase
    {
        #region Fields
        public readonly IMediator _mediator;
        private readonly ILogger<ApplicationUserController> _logger;
        #endregion

        #region Constructors
        public ApplicationUserController(IMediator mediator, ILogger<ApplicationUserController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        #endregion

        #region Handle Functions
        [HttpPost(Router.ApplicationUserRouting.Create)]
        public async Task<IActionResult> Create([FromBody] AddUserCommand command)
        {
            _logger.LogInformation("Create user request received");
            var response = await _mediator.Send(command);
            return NewResult(response);
        }

        [HttpGet(Router.ApplicationUserRouting.Paginated)]
        public async Task<IActionResult> Paginated([FromQuery] GetUserPaginationQuery query)
        {
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpGet(Router.ApplicationUserRouting.GetByID)]
        public async Task<IActionResult> GetUserByID([FromRoute] int id)
        {
            //var response = await _mediator.Send(new GetUserByIdQuery() {Id=id}); 
            //or
            return NewResult(await _mediator.Send(new GetUserByIdQuery(id)));
        }

        [HttpPut(Router.ApplicationUserRouting.Edit)]
        public async Task<IActionResult> Edit([FromBody] EditUserCommand command)
        {
            var response = await _mediator.Send(command);
            return NewResult(response);
        }

        [HttpDelete(Router.ApplicationUserRouting.Delete)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var response = await _mediator.Send(new DeleteUserCommand() { Id = id });
            return NewResult(response);
        }
        #endregion
    }
}
