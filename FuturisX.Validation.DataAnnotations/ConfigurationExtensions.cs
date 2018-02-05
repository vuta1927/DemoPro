using FuturisX.Configuration;
using FuturisX.Dependency;
using Microsoft.Extensions.DependencyInjection;

namespace FuturisX.Validation.DataAnnotations
{
    public static class ConfigurationExtensions
    {
        public static void UseDataAnnotations(this IValidationConfiguration configuration)
        {
            configuration.Configure.Services.AddTransient<IValidator, DataAnnotationsValidator>();
            configuration.Configure.Services.AddAll<IDataAnnotationsValidatorAdapter>(typeof(IDataAnnotationsValidatorAdapter).Assembly);
            configuration.Configure.Services.AddSingleton<IPropertyValidatorFactory, DefaultPropertyValidatorFactory>();
            configuration.Configure.Services.AddSingleton<IValidatableObjectAdapter, DefaultValidatableObjectAdapter>();
        }
    }
}