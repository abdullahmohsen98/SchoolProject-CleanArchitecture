using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.ApplicationUser.Queries.Modles;
using SchoolProject.Core.Features.ApplicationUser.Queries.Results;
using SchoolProject.Core.Resources;
using SchoolProject.Core.Wrappers;
using SchoolProject.Data.Entities.Identity;

namespace SchoolProject.Core.Features.ApplicationUser.Queries.Handlers
{
    public class UserQueryHandler : ResponseHandler,
                                    IRequestHandler<GetUserPaginationQuery, PaginatedResult<GetUserPaginationResponse>>,
                                    IRequestHandler<GetUserByIdQuery, Response<GetSingleUserResponse>>
    {
        #region Fields
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        private readonly UserManager<User> _userManager;
        #endregion

        #region Constructors
        public UserQueryHandler(IMapper mapper
                               , IStringLocalizer<SharedResources> stringLocalizer
                               , UserManager<User> userManager) : base(stringLocalizer)
        {
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
            _userManager = userManager;
        }
        #endregion

        #region Handler Functions
        public async Task<PaginatedResult<GetUserPaginationResponse>> Handle(GetUserPaginationQuery request, CancellationToken cancellationToken)
        {
            var users = _userManager.Users.AsQueryable();
            var paginatedList = await _mapper.ProjectTo<GetUserPaginationResponse>(users)
                                             .ToPaginatedListAsync(request.PageNubmer, request.PageSize);
            return paginatedList;
        }

        public async Task<Response<GetSingleUserResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            //var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == request.Id);
            if (user == null) return NotFound<GetSingleUserResponse>(_stringLocalizer[SharedResourceKeys.NotFound]);
            var result = _mapper.Map<GetSingleUserResponse>(user);
            return Success(result);
        }

        #endregion
    }
}
