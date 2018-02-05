using FuturisX.Configuration;

namespace FuturisX.Messaging
{
    /// <summary>
    /// Implementation of <see cref="IMessageConfiguration"/>
    /// </summary>
    public class MessageConfiguration : IMessageConfiguration
    {
        public IConfigure Configure { get; }

        public MessageConfiguration(IConfigure configure)
        {
            Configure = configure;
        }
    }
}