using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.Authorization.Queries.Models;
using SchoolProject.Core.Features.Authorization.Queries.Results;
using SchoolProject.Core.Resources;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Core.Features.Authorization.Queries.Handlers
{
    public class RoleQueryHandler : ResponseHandler,
                                    IRequestHandler<GetRolesListQuery, Response<List<GetRolesListResult>>>,
                                    IRequestHandler<GetRoleByIdQuery, Response<GetSingleRoleResult>>
    {
        #region Fields
        private readonly IAuthorizationSevice _authorizationSevice;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region Constructors
        public RoleQueryHandler(IAuthorizationSevice authorizationSevice,
                                IMapper mapper,
                                IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _authorizationSevice = authorizationSevice;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
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
        #endregion
    }
}
