using Microsoft.AspNetCore.Identity;
using SchoolProject.Data.DTOs;
using SchoolProject.Data.Entities.Identity;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Service.Implementations
{
    public class AuthorizationSevice : IAuthorizationSevice
    {
        #region Fields
        private readonly RoleManager<Role> _roleManager;
        #endregion

        #region Constructors
        public AuthorizationSevice(RoleManager<Role> roleManager)
        {
            _roleManager = roleManager;
        }
        #endregion

        #region Functions
        public async Task<string> AddRoleAsync(string roleName)
        {
            var identityRole = new Role();
            identityRole.Name = roleName;
            var result = await _roleManager.CreateAsync(identityRole);
            if (result.Succeeded)
                return "Success";
            return "Failed";
        }
        public async Task<bool> IsRoleExist(string roleName)
        {
            ////Check if the Role Exist or not:

            //var role = await _roleManager.FindByNameAsync(roleName);
            //if (role == null) return false;
            //return true;

            return await _roleManager.RoleExistsAsync(roleName);
        }
        public async Task<string> EditRoleAsync(EditRoleRequest request)
        {
            // Check if role exist
            var role = await _roleManager.FindByIdAsync(request.Id.ToString());
            // if not exist return notFound
            if (role == null) return "NotFound";
            // Check if the name is null or empty
            if (string.IsNullOrEmpty(request.Name)) return "InvalidName";
            // else Edit
            role.Name = request.Name;
            var result = await _roleManager.UpdateAsync(role);
            // return success
            if (result.Succeeded) return "Success";
            var errors = string.Join("-", result.Errors);
            return errors;
        }

        #endregion
    }
}
