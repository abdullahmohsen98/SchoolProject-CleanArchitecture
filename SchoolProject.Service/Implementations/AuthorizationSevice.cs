using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolProject.Data.DTOs;
using SchoolProject.Data.Entities.Identity;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Service.Implementations
{
    public class AuthorizationSevice : IAuthorizationSevice
    {
        #region Fields
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;
        #endregion

        #region Constructors
        public AuthorizationSevice(RoleManager<Role> roleManager,
                                   UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
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
        public async Task<bool> IsRoleExistByName(string roleName)
        {
            ////Check if the Role Exist or not:

            //var role = await _roleManager.FindByNameAsync(roleName);
            //if (role == null) return false;
            //return true;

            return await _roleManager.RoleExistsAsync(roleName);
        }
        public async Task<bool> IsRoleExistById(int roleId)
        {
            //Check if the Role Exist or not:
            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            if (role == null) return false;
            return true;
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
        public async Task<string> DeleteRoleAsync(int roleId)
        {
            // Check if role exist
            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            // if not exist return notFound
            if (role == null) return "NotFound";
            //Check if any user has this role
            var users = await _userManager.GetUsersInRoleAsync(role.Name);
            if (users.Count() > 0) return "RoleInUse";
            // else Delete
            var result = await _roleManager.DeleteAsync(role);
            // return success
            if (result.Succeeded) return "Success";
            //Problem
            var errors = string.Join("-", result.Errors);
            return errors;
        }
        public async Task<List<Role>> GetRolesListAsync()
        {
            return await _roleManager.Roles.ToListAsync();
        }
        public async Task<Role> GetRoleByIdAsync(int id)
        {
            return await _roleManager.FindByIdAsync(id.ToString());
        }
        public async Task<ManageUserRolesResult> ManageUserRolesAsync(User user)
        {
            var response = new ManageUserRolesResult();
            var rolesList = new List<UserRoles>();

            //Get UserRoles
            var userRoles = await _userManager.GetRolesAsync(user);

            //Get Roles
            var roles = await _roleManager.Roles.ToListAsync();
            response.UserId = user.Id;
            foreach (var role in roles)
            {
                var userRolesList = new UserRoles();
                userRolesList.Id = role.Id;
                userRolesList.Name = role.Name;
                if (userRoles.Contains(role.Name))
                {
                    userRolesList.HasRole = true;
                }
                else
                {
                    userRolesList.HasRole = false;
                }

                rolesList.Add(userRolesList);
            }
            response.UserRoles = rolesList;
            return response;
        }
        #endregion
    }
}
