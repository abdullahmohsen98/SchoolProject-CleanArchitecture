using MediatR;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
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
        private readonly TelemetryClient _telemetryClient;
        #endregion

        #region Constructors
        public ApplicationUserController(IMediator mediator, ILogger<ApplicationUserController> logger, TelemetryClient telemetryClient)
        {
            _mediator = mediator;
            _logger = logger;
            _telemetryClient = telemetryClient;
        }
        #endregion

        #region Handle Functions
        [HttpPost(Router.ApplicationUserRouting.Create)]
        public async Task<IActionResult> Create([FromBody] AddUserCommand command)
        {
            _logger.LogInformation("Create user request received");

            try
            {
                var response = await _mediator.Send(command);

                // Track successful user creation
                _logger.LogInformation("User created successfully");

                // Track a custom event
                var eventTelemetry = new EventTelemetry("CreateUserRequested");
                eventTelemetry.Properties["Category"] = "CustomCategory-CreateUser";
                eventTelemetry.Properties["SubCategory"] = "CustomCategory-CreateUser";
                _telemetryClient.TrackEvent(eventTelemetry);

                return NewResult(response);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Error occurred while creating user");

                // Track exception
                _telemetryClient.TrackException(ex);

                return BadRequest("Failed to create user");
            }
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
            try
            {
                var response = await _mediator.Send(command);

                // Track successful user edit
                _logger.LogInformation("User edited successfully");
                _telemetryClient.TrackEvent("UserEdited");

                return NewResult(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while editing user");

                // Track exception
                _telemetryClient.TrackException(ex);

                return BadRequest($"Failed to edit user");
            }
        }

        [HttpDelete(Router.ApplicationUserRouting.Delete)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var response = await _mediator.Send(new DeleteUserCommand() { Id = id });
            return NewResult(response);
        }

        [HttpPut(Router.ApplicationUserRouting.ChangePassword)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangeUserPasswordCommand command)
        {
            var response = await _mediator.Send(command);
            return NewResult(response);
        }
        #endregion
    }
}
