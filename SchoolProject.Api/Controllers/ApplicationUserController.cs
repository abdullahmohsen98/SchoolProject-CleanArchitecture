using MediatR;
using Microsoft.AspNetCore.Mvc;
using SchoolProject.Api.Base;
using SchoolProject.Core.Features.ApplicationUser.Commands.Models;
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
        #endregion
    }
}
