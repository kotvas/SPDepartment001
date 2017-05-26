using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SPDepartment001.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SPDepartment001.Models.DB
{
    public static class SeedIdentityData
    {
        const string DEFAULT_PASSWORD = "Password123!";

        private static UserManager<AppUser> userManager;

        public static void EnsurePopulated(IApplicationBuilder app)
        {
            userManager = app.ApplicationServices.GetRequiredService<UserManager<AppUser>>();

            CheckAndCreateUser("User001");
            CheckAndCreateUser("User002");
            CheckAndCreateUser("User003");
            CheckAndCreateUser("User004");
            CheckAndCreateUser("User005");
        }

        private static async void CheckAndCreateUser(string userName)
        {
            if (await userManager.FindByNameAsync(userName) == null)
            {
                AppUser user = new AppUser
                {
                    UserName = userName,
                    Email = $"{userName}@example.com"
                };

                IdentityResult result = await userManager.CreateAsync(user, DEFAULT_PASSWORD);
            }
        }
    }
}
