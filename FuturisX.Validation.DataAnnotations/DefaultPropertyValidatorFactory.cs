using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FuturisX.Validation.DataAnnotations
{
    /// <summary>
    /// Default implementation of the <see cref="IPropertyValidatorFactory"/> interface.
    /// </summary>
    public class DefaultPropertyValidatorFactory : IPropertyValidatorFactory
    {
        private readonly IEnumerable<IDataAnnotationsValidatorAdapter> _adapters;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultPropertyValidatorFactory"/> class.
        /// </summary>
        /// <param name="adapters">The <see cref="IDataAnnotationsValidatorAdapter"/> instances that are available to the factory.</param>
        public DefaultPropertyValidatorFactory(IEnumerable<IDataAnnotationsValidatorAdapter> adapters)
        {
            _adapters = adapters;
        }

        /// <summary>
        /// Gets the <see cref="PropertyValidator"/> instances for the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> that the validators should be retrieved for.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> instance, containing <see cref="IPropertyValidator"/> objects.</returns>
        public IEnumerable<IPropertyValidator> GetValidators(Type type)
        {
            var typeDescriptor = TypeDescriptor.GetProvider(type).GetTypeDescriptor(type);

            var results = new List<IPropertyValidator>();

            results.Add(GetTypeValidator(typeDescriptor));
            results.AddRange(GetPropertyValidators(typeDescriptor));

            return results;
        }

        private IEnumerable<PropertyValidator> GetPropertyValidators(ICustomTypeDescriptor typeDescriptor)
        {
            var propertyDescriptors = typeDescriptor.GetProperties();

            foreach (PropertyDescriptor descriptor in propertyDescriptors)
            {
                var attributes = descriptor.Attributes.OfType<ValidationAttribute>();

                var validator =
                    new PropertyValidator
                    {
                        AttributeAdaptors = GetAttributeAdaptors(attributes),
                        Descriptor = descriptor
                    };

                yield return validator;
            }
        }

        private PropertyValidator GetTypeValidator(ICustomTypeDescriptor typeDescriptor)
        {
            var classAttributes = typeDescriptor.GetAttributes().OfType<ValidationAttribute>();

            var classValidator =
                new PropertyValidator
                {
                    AttributeAdaptors = GetAttributeAdaptors(classAttributes)
                };
            return classValidator;
        }

        private IDictionary<ValidationAttribute, IEnumerable<IDataAnnotationsValidatorAdapter>> GetAttributeAdaptors(IEnumerable<ValidationAttribute> attributes)
        {
            var mappings =
                new Dictionary<ValidationAttribute, IEnumerable<IDataAnnotationsValidatorAdapter>>();

            foreach (var attribute in attributes)
            {
                var results = GetAdaptersForAttribute(attribute);
                mappings.Add(attribute, results);
            }

            return mappings;
        }

        private IEnumerable<IDataAnnotationsValidatorAdapter> GetAdaptersForAttribute(ValidationAttribute attribute)
        {
            return _adapters.Where(x => x.CanHandle(attribute));
        }
    }
}