using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.Authentication.Queries.Models;
using SchoolProject.Core.Resources;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Core.Features.Authentication.Queries.Handlers
{
    public class AuthenticationQueryHandler : ResponseHandler,
                                        IRequestHandler<AuthorizeUserQuery, Response<string>>
    {
        #region Fields
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        private readonly IAuthenticationService _authenticationService;
        #endregion

        #region Constructors
        public AuthenticationQueryHandler(IStringLocalizer<SharedResources> stringLocalizer,
                                    IMapper mapper,
                                    IAuthenticationService authenticationService) : base(stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
            _mapper = mapper;
            _authenticationService = authenticationService;
        }
        #endregion

        #region Handle Functions
        public async Task<Response<string>> Handle(AuthorizeUserQuery request, CancellationToken cancellationToken)
        {
            var result = await _authenticationService.ValidateToken(request.accessToken);
            if (result == "NotExpired")
                return Success(result);
            return Unauthorized<string>(_stringLocalizer[SharedResourceKeys.TokenIsExpired]);
        }
        #endregion
    }
}
