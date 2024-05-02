using Microsoft.EntityFrameworkCore;
using SchoolProject.Data.Entities;
using SchoolProject.Infrastructure.Abstracts;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Service.Implementations
{
    public class DepartmentService : IDepartmentService
    {
        #region Fields
        private readonly IDepartmentRepository _departmentRepository;
        #endregion

        #region Constructors
        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }
        #endregion

        #region Finctions
        public Task<Department> GetDepartmentById(int id)
        {
            var department = _departmentRepository.GetTableNoTracking().Where(d => d.DID.Equals(id))
                                                                       .Include(d => d.Instructors)
                                                                       .Include(d => d.Instructor)
                                                                       //.Include(d => d.Students)
                                                                       .Include(d => d.DepartmentSubjects).ThenInclude(d => d.Subject)
                                                                       .FirstOrDefaultAsync();
            return department;
        }

        public async Task<bool> IsDepartmentIdExist(int departmentId)
        {
            //Check if the Department Exist or not:
            return await _departmentRepository.GetTableNoTracking().AnyAsync(s => s.DID.Equals(departmentId));
        }

        #endregion
    }
}
