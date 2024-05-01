using Microsoft.EntityFrameworkCore;
using SchoolProject.Data.Entities;
using SchoolProject.Infrastructure.Abstracts;
using SchoolProject.Infrastructure.Data;
using SchoolProject.Infrastructure.InfrastructureBases;

namespace SchoolProject.Infrastructure.Repositories
{
    public class InstructorsRepository : GenericRepositoryAsync<Instructor>, IInstructorsRepository
    {
        #region Fields
        private DbSet<Instructor> _instructors;
        #endregion

        #region Constructors
        public InstructorsRepository(ApplicationDBContext dBContext) : base(dBContext)
        {
            _instructors = dBContext.Set<Instructor>();
        }
        #endregion

        #region Functions
        #endregion
    }
}
