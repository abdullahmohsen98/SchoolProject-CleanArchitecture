using Microsoft.AspNetCore.Identity;
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
        #endregion
    }
}
