using System.Threading.Tasks;
using FuturisX.Security.Permissions;
using Microsoft.AspNetCore.Authorization;

namespace FuturisX.AspNetCore.Mvc.Security.Permissions
{
    /// <summary>
    /// This authorization handler ensures that the user has the required permission.
    /// </summary>
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (!(bool)context?.User?.Identity?.IsAuthenticated)
            {
                return Task.CompletedTask;
            }
            if (context.User.HasClaim(Permission.ClaimType, requirement.Permission.Name))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}