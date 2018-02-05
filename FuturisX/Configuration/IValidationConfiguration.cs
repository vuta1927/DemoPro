using System;
using System.Collections.Generic;

namespace FuturisX.Configuration
{
    public interface IValidationConfiguration : IConfigurator
    {
        List<Type> IgnoredTypes { get; }
    }
}