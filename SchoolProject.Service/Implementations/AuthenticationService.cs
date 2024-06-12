using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SchoolProject.Data.Entities.Identity;
using SchoolProject.Data.Helpers;
using SchoolProject.Infrastructure.Abstracts;
using SchoolProject.Service.Abstracts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SchoolProject.Service.Implementations
{
    public class AuthenticationService : IAuthenticationService
    {
        #region Fields
        private readonly JwtSettings _jwtSettings;
        //private readonly ConcurrentDictionary<string, RefreshToken> _UserRefreshToken;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly UserManager<User> _userManager;
        #endregion

        #region Constructors
        public AuthenticationService(JwtSettings jwtSettings,
                                     IRefreshTokenRepository refreshTokenRepository,
                                     UserManager<User> userManager)
        {
            _jwtSettings = jwtSettings;
            //_UserRefreshToken = new ConcurrentDictionary<string, RefreshToken>();
            _refreshTokenRepository = refreshTokenRepository;
            _userManager = userManager;
        }
        #endregion

        #region Handle Functions
        public async Task<JwtAuthResult> GetJWTToken(User user)
        {
            var (jwtToken, accessToken) = GenerateJwtToken(user);
            var refreshToken = GetRefreshToken(user.UserName);
            var userRefreshToken = new UserRefreshToken
            {
                AddedTime = DateTime.Now,
                ExpiryDate = DateTime.Now.AddDays(_jwtSettings.RefreshTokenExpireDate),
                IsUsed = false,
                IsRevoked = false,
                JwtId = jwtToken.Id,
                RefreshToken = refreshToken.TokenString,
                Token = accessToken,
                UserId = user.Id,
            };
            await _refreshTokenRepository.AddAsync(userRefreshToken);
            var response = new JwtAuthResult();
            response.RefreshToken = refreshToken;
            response.AccessToken = accessToken;
            return response;
        }
        public async Task<JwtAuthResult> GetRefreshToken(User user, JwtSecurityToken jwtToken, DateTime? expiryDate, string refreshToken)
        {
            var (jwtSecurityToken, newToken) = GenerateJwtToken(user);

            var response = new JwtAuthResult();
            response.AccessToken = newToken;
            var refreshTokenResult = new RefreshToken();
            refreshTokenResult.UserName = jwtToken.Claims.FirstOrDefault(u => u.Type == nameof(UserClaimModel.UserName)).Value;
            refreshTokenResult.TokenString = refreshToken;
            refreshTokenResult.ExpiredAt = (DateTime)expiryDate;
            response.RefreshToken = refreshTokenResult;
            return response;
        }
        public async Task<(string, DateTime?)> ValidateDetails(JwtSecurityToken jwtToken, string accessToken, string refreshToken)
        {
            if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature))
            {
                return ("AlgorithmIsWrong", null);
            }
            if (jwtToken.ValidTo > DateTime.UtcNow)
            {
                return ("TokenIsNotExpired", null);
            }
            var userId = jwtToken.Claims.FirstOrDefault(u => u.Type == nameof(UserClaimModel.Id)).Value;
            var userRefreshToken = await _refreshTokenRepository.GetTableNoTracking()
                                              .FirstOrDefaultAsync(u => u.Token == accessToken &&
                                                                   u.RefreshToken == refreshToken &&
                                                                   u.UserId == int.Parse(userId));
            //Validations on token, accessToken
            if (userRefreshToken == null)
            {
                return ("RefreshTokenIsNotFound", null);
            }

            if (userRefreshToken.ExpiryDate < DateTime.UtcNow)
            {
                userRefreshToken.IsRevoked = true;
                userRefreshToken.IsUsed = false;
                await _refreshTokenRepository.UpdateAsync(userRefreshToken);
                return ("RefreshTokenIsExpired", null);
            }
            var expiryDate = userRefreshToken.ExpiryDate;
            return (userId, expiryDate);
        }

        private (JwtSecurityToken, string) GenerateJwtToken(User user)
        {
            var claims = GetClaims(user);
            var jwtToken = new JwtSecurityToken(
                                       _jwtSettings.Issuer,
                                       _jwtSettings.Audience,
                                       claims,
                                       expires: DateTime.UtcNow.AddDays(_jwtSettings.AccessTokenExpireDate),
                                       signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret)),
                                                                                  SecurityAlgorithms.HmacSha256Signature));
            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return (jwtToken, accessToken);
        }
        private RefreshToken GetRefreshToken(string userName)
        {
            var refreshToken = new RefreshToken
            {
                ExpiredAt = DateTime.UtcNow.AddMonths(_jwtSettings.RefreshTokenExpireDate),
                UserName = userName,
                TokenString = GenerateRefreshToken()
            };
            //_UserRefreshToken.AddOrUpdate(refreshToken.TokenString, refreshToken, (s, t) => refreshToken);
            return refreshToken;
        }
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            var randomNumberGenerate = RandomNumberGenerator.Create();
            randomNumberGenerate.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
        public List<Claim> GetClaims(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(nameof(UserClaimModel.UserName), user.UserName),
                new Claim(nameof(UserClaimModel.Email), user.Email),
                new Claim(nameof(UserClaimModel.PhoneNumber), user.PhoneNumber),
                new Claim(nameof(UserClaimModel.Id), user.Id.ToString())
            };
            return claims;
        }
        public JwtSecurityToken ReadJWTToken(string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                throw new ArgumentNullException(nameof(accessToken));
            }

            var handler = new JwtSecurityTokenHandler();
            var response = handler.ReadJwtToken(accessToken);
            return response;
        }
        public async Task<string> ValidateToken(string accessToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = _jwtSettings.ValidateIssuer,
                ValidIssuers = new[] { _jwtSettings.Issuer },
                ValidateIssuerSigningKey = _jwtSettings.ValidateIssuerSigningKey,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret)),
                ValidAudience = _jwtSettings.Audience,
                ValidateAudience = _jwtSettings.ValidateAudience,
                ValidateLifetime = _jwtSettings.ValidateLifeTime,
            };
            try
            {
                var validator = handler.ValidateToken(accessToken, parameters, out SecurityToken validatedToken);
                if (validator == null)
                {
                    return "InvalidToken";
                }
                return "Success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion
    }
}
