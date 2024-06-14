using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolProject.Data.Entities.Identity;

namespace SchoolProject.Infrastructure.Seeder
{
    public static class UserSeeder
    {
        public static async Task SeedAsync(UserManager<User> _userManager)
        {
            var userCount = await _userManager.Users.CountAsync();
            if (userCount <= 0)
            {
                var defaultUser = new User()
                {
                    FullName = "SchoolProject",
                    UserName = "admin",
                    Email = "admin@project.com",
                    Address = "Egypt",
                    Country = "Egypt",
                    PhoneNumber = "123456",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                };
                await _userManager.CreateAsync(defaultUser, "P@$$w0rd");
                await _userManager.AddToRoleAsync(defaultUser, "Admin");
            }
        }
    }
}
