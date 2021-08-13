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
            var userRoleExist = await UserManager.GetRolesAsync(await GetIdentityUser(user, _serviceProvider));

            if (roleExist && !userRoleExist.Contains(role))
            {
                await UserManager.AddToRoleAsync(await GetIdentityUser(user, _serviceProvider), role);
            }
            else
            {
                //Error message
            }
        }

        private async Task<IdentityUser> GetIdentityUser(string user, IServiceProvider _serviceProvider)
        {
            var UserManager = _serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            IdentityUser identityUser = await UserManager.FindByEmailAsync(user);

            return identityUser;
        }
    }
}
