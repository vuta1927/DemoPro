using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FuturisX.Helpers.Extensions;

namespace FuturisX.Validation.DataAnnotations
{
    /// <summary>
    /// Default adapter for models that implements the <see cref="IValidatableObject"/> interface.
    /// </summary>
    public class DefaultValidatableObjectAdapter : IValidatableObjectAdapter
    {
        public IEnumerable<ValidationError> Validate(object instance)
        {
            if (!(instance is IValidatableObject validateable))
            {
                yield break;
            }

            const string placeHolder = "*DISPLAYNAME_PLACEHOLDER*";
            // NOTE (from Mvc3 source): Container is never used here, because IValidatableObject doesn't give you
            // any way to get access to your container.
            // var context = new ValidationContext(container ?? instance, null, null);
            var context = new ValidationContext(instance, null, null)
            {
                DisplayName = placeHolder
            };

            var results = validateable.Validate(context);
            foreach (var result in results)
            {
                if (result != System.ComponentModel.DataAnnotations.ValidationResult.Success)
                {
                    if (result.MemberNames.IsNullOrEmpty())
                    {
                        yield return new ValidationError(null,
                            instance,
                            result.ErrorMessage.UnformatWith(placeHolder)); // Validatable objects should localize their error messages
                    }
                    else
                    {
                        foreach (var memberName in result.MemberNames)
                        {
                            yield return new ValidationError(memberName,
                                instance,
                                result.ErrorMessage.UnformatWith(placeHolder)); // Validatable objects should localize their error messages
                        }
                    }
                }
            }
        }
    }
}