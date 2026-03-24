using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using TechWorld.Data.Models;
using TechWorld.Data.Seeding.Interfaces;
using TechWorld.GCommon;

namespace TechWorld.Data.Seeding
{
    public class IdentitySeeder : IIdentitySeeder
    {
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentitySeeder
        (
            RoleManager<IdentityRole<Guid>> roleManager,
            UserManager<ApplicationUser> userManager
        )
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        private async Task SeedRolesAsync()
        {
            string[] roles =
            {
                ApplicationConstants.AdminRole,
                ApplicationConstants.UserRole
            };

            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole<Guid>(role));
                }
            }
        }

        private async Task SeedAdminAsync()
        {
            string adminEmail = "admin@techworld.com";
            string password = "аdmin123";

            var user = await _userManager.FindByEmailAsync(adminEmail);

            if (user == null)
            {
                var admin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail
                };

                await _userManager.CreateAsync(admin, password);

                await _userManager.AddToRoleAsync(admin, ApplicationConstants.AdminRole);
            }
        }

        public async Task SeedAsync()
        {
            await SeedRolesAsync();
            await SeedAdminAsync();
        }
    }
}
