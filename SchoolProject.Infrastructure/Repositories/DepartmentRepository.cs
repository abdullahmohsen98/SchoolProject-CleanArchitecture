using Microsoft.EntityFrameworkCore;
using SchoolProject.Data.Entities;
using SchoolProject.Infrastructure.Abstracts;
using SchoolProject.Infrastructure.Data;
using SchoolProject.Infrastructure.InfrastructureBases;

namespace SchoolProject.Infrastructure.Repositories
{
    public class DepartmentRepository : GenericRepositoryAsync<Department>, IDepartmentRepository
    {
        #region Fields
        private DbSet<Department> _departments;
        #endregion

        #region Constructors
        public DepartmentRepository(ApplicationDBContext dBContext) : base(dBContext)
        {
            _departments = dBContext.Set<Department>();
        }
        #endregion

        #region Functions
        #endregion

    }
}
