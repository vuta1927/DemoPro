using System.Security.Claims;
using System.Threading.Tasks;
using FuturisX.AspNetCore.Mvc.Security.Permissions;
using FuturisX.Security.Permissions;
using Microsoft.AspNetCore.Authorization;

namespace FuturisX.AspNetCore.Mvc.Security
{
    public static class AuthorizationServiceExtensions
    {
        public static Task<bool> AuthorizeAsync(this IAuthorizationService service, ClaimsPrincipal user,
            Permission permission)
        {
            return AuthorizeAsync(service, user, permission, null);
        }

        public static async Task<bool> AuthorizeAsync(this IAuthorizationService service, ClaimsPrincipal user,
            Permission permission, object resource)
        {
            return (await service.AuthorizeAsync(user, resource, new PermissionRequirement(permission))).Succeeded;
        }
    }
}