﻿using SchoolProject.Data.DTOs;
using SchoolProject.Data.Entities.Identity;
using SchoolProject.Data.Results;

namespace SchoolProject.Service.Abstracts
{
    public interface IAuthorizationSevice
    {
        public Task<string> AddRoleAsync(string roleName);
        public Task<bool> IsRoleExistByName(string roleName);
        public Task<bool> IsRoleExistById(int roleId);
        public Task<string> EditRoleAsync(EditRoleRequest request);
        public Task<string> DeleteRoleAsync(int roleId);
        public Task<List<Role>> GetRolesListAsync();
        public Task<Role> GetRoleByIdAsync(int id);
        public Task<ManageUserRolesResult> ManageUserRolesAsync(User user);
        public Task<string> UpdateUserRoles(ManageUserRolesRequest request);
        public Task<ManageUserClaimsResult> ManageUserClaimsAsync(User user);
    }
}
