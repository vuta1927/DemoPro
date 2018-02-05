using System.Threading.Tasks;

namespace FuturisX.Security.Roles
{
    public interface IRoleRemovedEventHandler
    {
        Task RoleRemovedAsync(string roleName);
    }
}