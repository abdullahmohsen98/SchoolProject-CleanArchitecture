using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolProject.Api.Base;
using SchoolProject.Core.Features.Authorization.Commands.Models;
using SchoolProject.Data.AppMetaData;

namespace SchoolProject.Api.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    //[Authorize(Roles = "Admin,User")] // Admin or User
    //[Authorize(Roles = "Admin")] [Authorize(Roles = "User")] // Admin and User
    public class AuthorizationController : AppControllerBase
    {
        #region Fields
        public readonly IMediator _mediator;
        private readonly ILogger<AuthorizationController> _logger;
        #endregion

        #region Constructors
        public AuthorizationController(IMediator mediator, ILogger<AuthorizationController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        #endregion

        #region Handle Functions
        [HttpPost(Router.AuthorizationRouting.Create)]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Create([FromForm] AddRoleCommand command)
        {
            _logger.LogInformation("Create role received");
            var response = await _mediator.Send(command);
            return NewResult(response);
        }
        [HttpPut(Router.AuthorizationRouting.Edit)]

        public async Task<IActionResult> Edit([FromBody] EditRoleCommand command)
        {
            var response = await _mediator.Send(command);
            return NewResult(response);
        }

        #endregion
    }
}
