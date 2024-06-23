using SchoolProject.Data.DTOs;

namespace SchoolProject.Service.Abstracts
{
    public interface IAuthorizationSevice
    {
        public Task<string> AddRoleAsync(string roleName);
        public Task<bool> IsRoleExist(string roleName);
        public Task<string> EditRoleAsync(EditRoleRequest request);

    }
}
