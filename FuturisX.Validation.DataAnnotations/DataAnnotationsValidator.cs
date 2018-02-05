using System.Collections.Generic;

namespace FuturisX.Validation.DataAnnotations
{
    /// <summary>
    /// The default Data Annotations implementation of <see cref="IValidator"/>.
    /// </summary>
    public class DataAnnotationsValidator : IValidator
    {
        private readonly IValidatableObjectAdapter _validatableObjectAdapter;
        private readonly IPropertyValidatorFactory _propertyValidatorFactory;

        public DataAnnotationsValidator(IValidatableObjectAdapter validatableObjectAdapter, IPropertyValidatorFactory propertyValidatorFactory)
        {
            _validatableObjectAdapter = validatableObjectAdapter;
            _propertyValidatorFactory = propertyValidatorFactory;
        }

        public IEnumerable<ValidationError> Validate(object instance)
        {
            var validators = _propertyValidatorFactory.GetValidators(instance.GetType());
            foreach (var propertyValidator in validators)
            {
                var results = propertyValidator.Validate(instance);
                foreach (var result in results)
                {
                    yield return result;
                }
            }

            foreach (var validationError in _validatableObjectAdapter.Validate(instance))
            {
                yield return validationError;
            }
        }
    }
}