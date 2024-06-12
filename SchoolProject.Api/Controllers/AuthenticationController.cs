using MediatR;
using Microsoft.AspNetCore.Mvc;
using SchoolProject.Api.Base;
using SchoolProject.Core.Features.Authentication.Commands.Models;
using SchoolProject.Core.Features.Authentication.Queries.Models;
using SchoolProject.Data.AppMetaData;

namespace SchoolProject.Api.Controllers
{
    [ApiController]
    public class AuthenticationController : AppControllerBase
    {
        #region Fields
        private readonly IMediator _mediator;
        #endregion

        #region Constructors
        public AuthenticationController(IMediator mediator)
        {
            _mediator = mediator;
        }
        #endregion

        #region Handle Functions
        [HttpPost(Router.AuthenticationRouting.SignIn)]
        public async Task<IActionResult> SignIn([FromForm] SignInCommand command)
        {
            var response = await _mediator.Send(command);
            return NewResult(response);
        }

        [HttpPost(Router.AuthenticationRouting.RefreshToken)]
        public async Task<IActionResult> RefreshToken([FromForm] RefreshTokenCommand command)
        {
            var response = await _mediator.Send(command);
            return NewResult(response);
        }

        [HttpGet(Router.AuthenticationRouting.ValidateToken)]
        public async Task<IActionResult> ValidateToken([FromQuery] AuthorizeUserQuery query)
        {
            var response = await _mediator.Send(query);
            return NewResult(response);
        }
        #endregion
    }
}
