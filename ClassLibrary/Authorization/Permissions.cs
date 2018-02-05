using System;
using System.Collections.Generic;
using FuturisX.Security;
using FuturisX.Security.Permissions;

namespace myCore.Authorization
{
    public class Permissions : IPermissionProvider
    {
        public static readonly Permission AdminPermission = new Permission(DemoPermission.Administrator);
        public static readonly Permission Page = new Permission(DemoPermission.Page, children: new[] { AdminPermission });

        public IEnumerable<Permission> GetPermissions()
        {
            return new[]
            {
                Page,
                AdminPermission
            };
        }

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes()
        {
            return new[]
            {
                new PermissionStereotype
                {
                    Name = "Administrator",
                    Permissions = new[] {Page, AdminPermission}
                }
            };
        }
    }
}
