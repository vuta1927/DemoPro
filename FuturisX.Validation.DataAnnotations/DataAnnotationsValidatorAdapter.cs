using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FuturisX.Helpers.Extensions;

namespace FuturisX.Validation.DataAnnotations
{
    /// <summary>
    /// A default implementation of an <see cref="IDataAnnotationsValidatorAdapter"/>.
    /// </summary>
    public abstract class DataAnnotationsValidatorAdapter : IDataAnnotationsValidatorAdapter
    {
        public abstract bool CanHandle(ValidationAttribute attribute);

        public IEnumerable<ValidationError> Validate(object instance, ValidationAttribute attribute, PropertyDescriptor descriptor)
        {
            const string placeHolder = "*DISPLAYNAME_PLACEHOLDER*";
            var validationContext = new ValidationContext(instance, null, null)
                {
                    MemberName = descriptor == null ? null : descriptor.Name,
                    DisplayName = placeHolder
                };

            if (descriptor != null)
            {
                // Display(Name) will auto populate the context, while DisplayName() needs to be manually set
                if (validationContext.MemberName == validationContext.DisplayName && !string.IsNullOrEmpty(descriptor.DisplayName))
                {
                    validationContext.DisplayName = descriptor.DisplayName;
                }

                instance = descriptor.GetValue(instance);
            }

            var result = attribute.GetValidationResult(instance, validationContext);
            if (result != null && result != System.ComponentModel.DataAnnotations.ValidationResult.Success)
            {
                yield return new ValidationError(validationContext.MemberName,
                    instance,
                    result.ErrorMessage.UnformatWith(placeHolder));
            }
        }
    }
}