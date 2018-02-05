using System.Threading.Tasks;

namespace FuturisX.Messaging.Events
{
    public interface IEventService
    {
        Task SaveEventAsync(IEvent @event);
        Task MarkEventAsPublishedAsync(IEvent @event);
    }
}