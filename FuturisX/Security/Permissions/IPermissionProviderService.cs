using System.Collections.Generic;

namespace FuturisX.Security.Permissions
{
    public interface IPermissionProviderService
    {
        IEnumerable<Permission> GetPermissions();
        Permission GetPermissionBy(string name);
    }
}