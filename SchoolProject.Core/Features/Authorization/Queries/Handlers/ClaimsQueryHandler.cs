using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.Authorization.Queries.Models;
using SchoolProject.Core.Resources;
using SchoolProject.Data.Entities.Identity;
using SchoolProject.Data.Results;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Core.Features.Authorization.Queries.Handlers
{
    public class ClaimsQueryHandler : ResponseHandler,
                                    IRequestHandler<ManageUserClaimsQuery, Response<ManageUserClaimsResult>>
    {
        #region Fields
        private readonly IAuthorizationSevice _authorizationSevice;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        private readonly UserManager<User> _userManager;
        #endregion

        #region Constructors
        public ClaimsQueryHandler(IAuthorizationSevice authorizationSevice,
                                IStringLocalizer<SharedResources> stringLocalizer,
                                UserManager<User> userManager) : base(stringLocalizer)
        {
            _authorizationSevice = authorizationSevice;
            _stringLocalizer = stringLocalizer;
            _userManager = userManager;
        }

        #endregion

        #region HandleFunctions
        public async Task<Response<ManageUserClaimsResult>> Handle(ManageUserClaimsQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null) return NotFound<ManageUserClaimsResult>(_stringLocalizer[SharedResourceKeys.UserNotFound]);
            var result = await _authorizationSevice.ManageUserClaimsAsync(user);
            return Success(result);
        }
        #endregion
    }
}
