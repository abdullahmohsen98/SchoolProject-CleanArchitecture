using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolProject.Api.Base;
using SchoolProject.Core.Features.Authorization.Commands.Models;
using SchoolProject.Core.Features.Authorization.Queries.Models;
using SchoolProject.Data.AppMetaData;
using Swashbuckle.AspNetCore.Annotations;

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

        [HttpDelete(Router.AuthorizationRouting.Delete)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var response = await _mediator.Send(new DeleteRoleCommand() { Id = id });
            return NewResult(response);
        }

        [HttpGet(Router.AuthorizationRouting.RolesList)]
        public async Task<IActionResult> GetRolesList()
        {
            var response = await _mediator.Send(new GetRolesListQuery());
            return NewResult(response);
        }

        [SwaggerOperation(Summary = "Idالصلاحية عن طريق ال", OperationId = "RoleById")]
        [HttpGet(Router.AuthorizationRouting.GetRoleByID)]
        public async Task<IActionResult> GetRoleByID([FromRoute] int id)
        {
            var response = await _mediator.Send(new GetRoleByIdQuery(id));
            return NewResult(response);
        }

        [SwaggerOperation(Summary = "إدارة صلاحيات المستخدمين", OperationId = "ManageUserRoles")]
        [HttpGet(Router.AuthorizationRouting.ManageUserRoles)]
        public async Task<IActionResult> ManageUserRoles([FromRoute] int userId)
        {
            var response = await _mediator.Send(new ManageUserRolesQuery() { UserId = userId });
            return NewResult(response);
        }

        [SwaggerOperation(Summary = "تعديل صلاحيات المستخدمين", OperationId = "UpdateUserRoles")]
        [HttpPost(Router.AuthorizationRouting.UpdateUserRoles)]
        public async Task<IActionResult> UpdateUserRoles([FromBody] UpdateUserRolesCommand command)
        {
            var response = await _mediator.Send(command);
            return NewResult(response);
        }

        [SwaggerOperation(Summary = "إدارة صلاحيات الإستخدام للمستخدمين", OperationId = "ManageUserClaims")]
        [HttpGet(Router.AuthorizationRouting.ManageUserClaims)]
        public async Task<IActionResult> ManageUserClaims([FromRoute] int userId)
        {
            var response = await _mediator.Send(new ManageUserClaimsQuery() { UserId = userId });
            return NewResult(response);
        }
        #endregion
    }
}
