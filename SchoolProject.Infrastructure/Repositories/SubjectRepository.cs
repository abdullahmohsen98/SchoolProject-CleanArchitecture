using Microsoft.EntityFrameworkCore;
using SchoolProject.Data.Entities;
using SchoolProject.Infrastructure.Abstracts;
using SchoolProject.Infrastructure.Data;
using SchoolProject.Infrastructure.InfrastructureBases;

namespace SchoolProject.Infrastructure.Repositories
{
    public class SubjectRepository : GenericRepositoryAsync<Subjects>, ISubjectRepository
    {
        #region Fields
        private DbSet<Subjects> _subjects;
        #endregion

        #region Constructors
        public SubjectRepository(ApplicationDBContext dBContext) : base(dBContext)
        {
            _subjects = dBContext.Set<Subjects>();
        }
        #endregion

        #region Functions
        #endregion
    }
}
