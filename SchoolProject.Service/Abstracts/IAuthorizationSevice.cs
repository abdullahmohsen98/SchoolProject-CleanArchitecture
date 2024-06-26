using SchoolProject.Data.DTOs;

namespace SchoolProject.Service.Abstracts
{
    public interface IAuthorizationSevice
    {
        public Task<string> AddRoleAsync(string roleName);
        public Task<bool> IsRoleExistByName(string roleName);
        public Task<bool> IsRoleExistById(int roleId);
        public Task<string> EditRoleAsync(EditRoleRequest request);
        public Task<string> DeleteRoleAsync(int roleId);

    }
}
