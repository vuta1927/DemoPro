using System.Threading.Tasks;

namespace FuturisX.Messaging.Handling
{
    public interface IDynamicEventHandler
    {
        Task HandleAsync(dynamic eventData);
    }
}