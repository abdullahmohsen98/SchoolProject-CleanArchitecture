using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.Authorization.Queries.Models;
using SchoolProject.Core.Features.Authorization.Queries.Results;
using SchoolProject.Core.Resources;
using SchoolProject.Data.Entities.Identity;
using SchoolProject.Data.Results;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Core.Features.Authorization.Queries.Handlers
{
    public class RoleQueryHandler : ResponseHandler,
                                    IRequestHandler<GetRolesListQuery, Response<List<GetRolesListResult>>>,
                                    IRequestHandler<GetRoleByIdQuery, Response<GetSingleRoleResult>>,
                                    IRequestHandler<ManageUserRolesQuery, Response<ManageUserRolesResult>>
    {
        #region Fields
        private readonly IAuthorizationSevice _authorizationSevice;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        private readonly UserManager<User> _userManager;
        #endregion

        #region Constructors
        public RoleQueryHandler(IAuthorizationSevice authorizationSevice,
                                IMapper mapper,
                                IStringLocalizer<SharedResources> stringLocalizer,
                                UserManager<User> userManager) : base(stringLocalizer)
        {
            _authorizationSevice = authorizationSevice;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
            _userManager = userManager;
        }
        #endregion

        #region HandleFunctions
        public async Task<Response<List<GetRolesListResult>>> Handle(GetRolesListQuery request, CancellationToken cancellationToken)
        {
            var roleList = await _authorizationSevice.GetRolesListAsync();
            var result = _mapper.Map<List<GetRolesListResult>>(roleList);
            return Success(result);
        }

        public async Task<Response<GetSingleRoleResult>> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
        {
            var role = await _authorizationSevice.GetRoleByIdAsync(request.Id);
            if (role == null) return NotFound<GetSingleRoleResult>(_stringLocalizer[SharedResourceKeys.NotFound]);
            var result = _mapper.Map<GetSingleRoleResult>(role);
            return Success(result);
        }

        public async Task<Response<ManageUserRolesResult>> Handle(ManageUserRolesQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null) return NotFound<ManageUserRolesResult>(_stringLocalizer[SharedResourceKeys.UserNotFound]);
            var result = await _authorizationSevice.ManageUserRolesAsync(user);
            return Success(result);
        }
        #endregion
    }
}
