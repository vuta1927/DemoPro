using System.Collections.Generic;

namespace FuturisX.Messaging.Commands
{
    public interface ICommandBus
    {
        void Send(Envelope<ICommand> command);
        void Send(IEnumerable<Envelope<ICommand>> commands);
    }
}