

using MediatR;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.Authorization.Commands.Models;
using SchoolProject.Core.Resources;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Core.Features.Authorization.Commands.Handlers
{
    public class RoleCommandHandler : ResponseHandler, IRequestHandler<AddRoleCommad, Response<string>>
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
        public async Task<Response<string>> Handle(AddRoleCommad request, CancellationToken cancellationToken)
        {
            var result = await _authorizationSevice.AddRoleAsync(request.RoleName);
            if (result == "Success") return Success("");
            return BadRequest<string>(_stringLocalizer[SharedResourceKeys.FailedToAddRole]);
        }
        #endregion
    }
}
