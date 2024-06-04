using Microsoft.EntityFrameworkCore;
using SchoolProject.Data.Entities.Identity;
using SchoolProject.Infrastructure.Abstracts;
using SchoolProject.Infrastructure.Data;
using SchoolProject.Infrastructure.InfrastructureBases;

namespace SchoolProject.Infrastructure.Repositories
{
    public class RefreshTokenRepository : GenericRepositoryAsync<UserRefreshToken>, IRefreshTokenRepository
    {
        #region Fields
        private DbSet<UserRefreshToken> _userRefreshTokens;
        #endregion

        #region Constructors
        public RefreshTokenRepository(ApplicationDBContext dBContext) : base(dBContext)
        {
            _userRefreshTokens = dBContext.Set<UserRefreshToken>();
        }
        #endregion

        #region Functions
        #endregion
    }
}
