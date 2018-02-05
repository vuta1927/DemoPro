using FuturisX.Dependency;
using Microsoft.Extensions.Primitives;

namespace FuturisX.Caching
{
    public interface ISignal : ISingletonDependency
    {
        IChangeToken GetToken(string key);

        void SignalToken(string key);
    }
}
