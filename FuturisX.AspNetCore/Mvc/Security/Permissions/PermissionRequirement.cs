using FuturisX.Helpers.Exception;
using FuturisX.Security.Permissions;
using Microsoft.AspNetCore.Authorization;

namespace FuturisX.AspNetCore.Mvc.Security.Permissions
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public PermissionRequirement(Permission permission)
        {
            Throw.IfArgumentNull(permission, nameof(permission));
            Permission = permission;
        }

        public Permission Permission { get; set; }
    }
}