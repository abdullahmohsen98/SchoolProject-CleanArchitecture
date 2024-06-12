using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.Authentication.Commands.Models;
using SchoolProject.Core.Resources;
using SchoolProject.Data.Entities.Identity;
using SchoolProject.Data.Helpers;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Core.Features.Authentication.Commands.Handlers
{
    public class AuthenticationCommandHandler : ResponseHandler,
                                        IRequestHandler<SignInCommand, Response<JwtAuthResult>>,
                                        IRequestHandler<RefreshTokenCommand, Response<JwtAuthResult>>
    {
        #region Fields
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IAuthenticationService _authenticationService;
        private readonly ILogger<AuthenticationCommandHandler> _logger;
        #endregion

        #region Constructors
        public AuthenticationCommandHandler(IStringLocalizer<SharedResources> stringLocalizer,
                                    IMapper mapper,
                                    UserManager<User> userManager,
                                    SignInManager<User> signInManager,
                                    IAuthenticationService authenticationService,
                                    ILogger<AuthenticationCommandHandler> logger) : base(stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _authenticationService = authenticationService;
            _logger = logger;
        }
        #endregion

        #region Handle Functions
        public async Task<Response<JwtAuthResult>> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling sign-in for user: {UserName}", request.UserName);

            // Check if user exists
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                _logger.LogWarning("User not found: {UserName}", request.UserName);
                return BadRequest<JwtAuthResult>(_stringLocalizer[SharedResourceKeys.UserNameIsNotExist]);
            }

            // Check if email is confirmed
            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                _logger.LogWarning("Email not confirmed for user: {UserName}", request.UserName);
                return BadRequest<JwtAuthResult>(_stringLocalizer[SharedResourceKeys.EmailNotConfirmed]);
            }

            // Check if the user is locked out
            if (await _userManager.IsLockedOutAsync(user))
            {
                _logger.LogWarning("Account locked for user: {UserName}", request.UserName);
                return BadRequest<JwtAuthResult>(_stringLocalizer[SharedResourceKeys.AccountLocked]);
            }

            // Try to sign in
            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!signInResult.Succeeded)
            {
                var reason = signInResult.IsLockedOut ? "Locked out" : signInResult.IsNotAllowed ? "Not allowed" : "Failed";
                _logger.LogWarning("Sign-in failed for user: {UserName}. Reason: {Reason}", request.UserName, reason);
                return BadRequest<JwtAuthResult>(_stringLocalizer[SharedResourceKeys.PasswordIsNotCorrect]);
            }

            // Generate token
            var accessToken = await _authenticationService.GetJWTToken(user);
            _logger.LogInformation("Generated JWT token for user: {UserName}", request.UserName);

            // Return token
            return Success(accessToken);
        }

        public async Task<Response<JwtAuthResult>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var jwtToken = _authenticationService.ReadJWTToken(request.AccessToken);
            var userIdAndExpiryDate = await _authenticationService.ValidateDetails(jwtToken, request.AccessToken, request.RefreshToken);
            switch (userIdAndExpiryDate)
            {
                case ("AlgorithmIsWrong", null): return Unauthorized<JwtAuthResult>(_stringLocalizer[SharedResourceKeys.AlgorithmIsWrong]);
                case ("TokenIsNotExpired", null): return Unauthorized<JwtAuthResult>(_stringLocalizer[SharedResourceKeys.TokenIsNotExpired]);
                case ("RefreshTokenIsNotFound", null): return Unauthorized<JwtAuthResult>(_stringLocalizer[SharedResourceKeys.RefreshTokenIsNotFound]);
                case ("RefreshTokenIsExpired", null): return Unauthorized<JwtAuthResult>(_stringLocalizer[SharedResourceKeys.RefreshTokenIsExpired]);
            }
            var (userId, expiryDate) = userIdAndExpiryDate;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound<JwtAuthResult>();

            //Generate refresh token
            var result = await _authenticationService.GetRefreshToken(user, jwtToken, expiryDate, request.RefreshToken);
            return Success(result);
        }

        #endregion
    }
}
