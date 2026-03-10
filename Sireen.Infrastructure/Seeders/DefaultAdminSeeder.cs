using Microsoft.AspNetCore.Identity;
using Sireen.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sireen.Infrastructure.Seeders
{
    public class DefaultAdminSeeder
    {
        private readonly UserManager<AppUser> _userManager;

        public DefaultAdminSeeder(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task SeedAsync()
        {            
            var adminEmail = "nextcoder41@gmail.com";
            var adminUser = await _userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new AppUser
                {
                    UserName = "admin",
                    Email = adminEmail,
                    EmailConfirmed = true
                };
                var result = await _userManager.CreateAsync(adminUser, "Admin@123");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
}
