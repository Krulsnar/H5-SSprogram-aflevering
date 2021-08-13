using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace H5_SSP_aflevering.Areas.Identity.Code
{
    public class RoleHandler
    {
        public async Task CreateRole(string role, IServiceProvider _serviceProvider)
        {
            var RoleManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var roleExist = await RoleManager.RoleExistsAsync(role);

            if (!roleExist)
            {
                IdentityResult roleResult = await RoleManager.CreateAsync(new IdentityRole(role));
            }
        }

        public async Task SetRole(string user, string role, IServiceProvider _serviceProvider)
        {
            var UserManager = _serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var RoleManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            
            var roleExist = await RoleManager.RoleExistsAsync(role);

            if (roleExist)
            {
                IdentityUser identityUser = await UserManager.FindByEmailAsync(user);
                await UserManager.AddToRoleAsync(identityUser, role);
            }
            else
            {
                //Error message
            }
        }
    }
}
