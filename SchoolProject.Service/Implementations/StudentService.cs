using Microsoft.EntityFrameworkCore;
using SchoolProject.Data.Entities;
using SchoolProject.Data.Helpers;
using SchoolProject.Infrastructure.Abstracts;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Service.Implementations
{
    public class StudentService : IStudentService
    {
        #region Fields
        private readonly IStudentRepository _studentRepository;

        #endregion

        #region Construcrors
        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        #endregion

        #region Handle Functions
        public async Task<List<Student>> GetStudentsListAsync()
        {
            return await _studentRepository.GetStudentsListAsync();
        }

        public async Task<Student> GetStudentByIdWithIncludeAsync(int id)
        {
            //var student = await _studentRepository.GetStudentsListAsync();
            var student = _studentRepository.GetTableNoTracking()
                                            .Include(s => s.Department)
                                            .Where(s => s.StudID.Equals(id))
                                            .FirstOrDefault();

            return student;
        }

        public async Task<string> AddAsync(Student student)
        {
            //Add Student:
            await _studentRepository.AddAsync(student);
            await _studentRepository.SaveChangesAsync();
            return "Success";
        }

        public async Task<bool> IsNameArExist(string nameAr)
        {
            //Check if the name Exist or not:
            var student = _studentRepository.GetTableNoTracking().Where(s => s.NameAr.Equals(nameAr)).FirstOrDefault();
            if (student == null) return false;
            return true;
        }

        public async Task<bool> IsNameEnExist(string nameEn)
        {
            //Check if the name Exist or not:
            var student = _studentRepository.GetTableNoTracking().Where(s => s.NameEn.Equals(nameEn)).FirstOrDefault();
            if (student == null) return false;
            return true;
        }
        public async Task<bool> IsNameArExistExcludeSelf(string nameAr, int id)
        {
            //Check if the name Exist or not:
            var student = await _studentRepository.GetTableNoTracking().Where(s => s.NameAr.Equals(nameAr) && !s.StudID.Equals(id)).FirstOrDefaultAsync();
            if (student == null) return false;
            return true;
        }
        public async Task<bool> IsNameEnExistExcludeSelf(string nameEn, int id)
        {
            //Check if the name Exist or not:
            var student = await _studentRepository.GetTableNoTracking().Where(s => s.NameEn.Equals(nameEn) && !s.StudID.Equals(id)).FirstOrDefaultAsync();
            if (student == null) return false;
            return true;
        }
        public async Task<string> EditAsync(Student student)
        {
            await _studentRepository.UpdateAsync(student);
            return "Success";
        }

        public async Task<string> DeleteAsync(Student student)
        {
            var trans = _studentRepository.BeginTransaction();
            try
            {
                await _studentRepository.DeleteAsync(student);
                await trans.CommitAsync();
                return "Success";
            }
            catch (Exception)
            {
                await trans.RollbackAsync();
            }
            return "Failed";
        }

        public async Task<Student> GetByIdAsync(int id)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            return student;
        }

        public IQueryable<Student> GetStudentsQuerable()
        {
            return _studentRepository.GetTableNoTracking().Include(s => s.Department).AsQueryable();
        }

        public IQueryable<Student> FilterStudentPaginatedQuerable(StudentOrderingEnum orderingEnum, string search)
        {
            var querable = _studentRepository.GetTableNoTracking().Include(s => s.Department).AsQueryable();
            if (search != null)
            {
                querable = querable.Where(s => s.Localize(s.NameEn, s.NameAr).Contains(search) || s.Address.Contains(search));
            }

            switch (orderingEnum)
            {
                case StudentOrderingEnum.StudID:
                    querable = querable.OrderBy(s => s.StudID);
                    break;
                case StudentOrderingEnum.Name:
                    querable = querable.OrderBy(s => s.Localize(s.NameEn, s.NameAr));
                    break;
                case StudentOrderingEnum.Adddress:
                    querable = querable.OrderBy(s => s.Address);
                    break;
                case StudentOrderingEnum.DepartmentName:
                    querable = querable.OrderBy(s => s.Department.DNameAr);
                    break;
                default:
                    break;
            }
            return querable;
        }

        public IQueryable<Student> GetStudentsByDepartmentIDQuerable(int DID)
        {
            return _studentRepository.GetTableNoTracking().Where(s => s.DID.Equals(DID)).AsQueryable();
        }

        #endregion
    }
}
