using System.Threading.Tasks;
using FuturisX.Security;
using FuturisX.Security.Roles;
using Microsoft.AspNetCore.Identity;

namespace FuturisX.AspNetCore.Mvc.Security
{
    public class UserRoleRemovedEventHandler : IRoleRemovedEventHandler
    {
        private readonly UserManager<User> _userManager;

        public UserRoleRemovedEventHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task RoleRemovedAsync(string roleName)
        {
            var users = await _userManager.GetUsersInRoleAsync(roleName);

            foreach (var user in users)
            {
                await _userManager.RemoveFromRoleAsync(user, roleName);
            }
        }
    }
}