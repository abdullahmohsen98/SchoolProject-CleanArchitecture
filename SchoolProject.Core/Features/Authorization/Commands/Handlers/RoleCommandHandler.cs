

using MediatR;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.Authorization.Commands.Models;
using SchoolProject.Core.Resources;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Core.Features.Authorization.Commands.Handlers
{
    public class RoleCommandHandler : ResponseHandler,
                                      IRequestHandler<AddRoleCommand, Response<string>>,
                                      IRequestHandler<EditRoleCommand, Response<string>>,
                                      IRequestHandler<DeleteRoleCommand, Response<string>>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        private readonly IAuthorizationSevice _authorizationSevice;
        #endregion

        #region Constructors
        public RoleCommandHandler(IStringLocalizer<SharedResources> stringLocalizer,
                                  IAuthorizationSevice authorizationSevice) : base(stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
            _authorizationSevice = authorizationSevice;
        }
        #endregion

        #region Handle Functions
        public async Task<Response<string>> Handle(AddRoleCommand request, CancellationToken cancellationToken)
        {
            var result = await _authorizationSevice.AddRoleAsync(request.RoleName);
            if (result == "Success") return Success("");
            return BadRequest<string>(_stringLocalizer[SharedResourceKeys.FailedToAddRole]);
        }
        public async Task<Response<string>> Handle(EditRoleCommand command, CancellationToken cancellationToken)
        {
            var result = await _authorizationSevice.EditRoleAsync(command);
            if (result == "NotFound") return NotFound<string>();
            else if (result == "Success") return Success((string)_stringLocalizer[SharedResourceKeys.Updated]);
            //else return BadRequest<string>(_stringLocalizer[SharedResourceKeys.UpdateFailed]);
            else return BadRequest<string>(result);
        }

        public async Task<Response<string>> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            var result = await _authorizationSevice.DeleteRoleAsync(request.Id);
            if (result == "NotFound") return NotFound<string>();
            else if (result == "RoleInUse") return BadRequest<string>(_stringLocalizer[SharedResourceKeys.RoleInUse]);
            else if (result == "Success") return Success((string)_stringLocalizer[SharedResourceKeys.Deleted]);
            //else return BadRequest<string>(_stringLocalizer[SharedResourceKeys.DeletedFailed]);
            else return BadRequest<string>(result);
        }

        #endregion
    }
}
