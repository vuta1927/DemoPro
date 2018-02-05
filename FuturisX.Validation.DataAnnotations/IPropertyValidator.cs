using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FuturisX.Validation.DataAnnotations
{
    public interface IPropertyValidator
    {
        /// <summary>
        /// Gets or sets the <see cref="IDataAnnotationsValidatorAdapter"/> instances that should be associated with
        /// each of the <see cref="ValidationAttribute"/> that are specified for the property that is being validated.
        /// </summary>
        IDictionary<ValidationAttribute, IEnumerable<IDataAnnotationsValidatorAdapter>> AttributeAdaptors { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="PropertyDescriptor"/> for the property that is being validated.
        /// </summary>
        PropertyDescriptor Descriptor { get; set; }

        /// <summary>
        /// Gets the validation result for the specified <paramref name="instance"/>.
        /// </summary>
        /// <param name="instance">The instance that should be validated.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> instance, containing <see cref="ValidationError"/> objects.</returns>
        IEnumerable<ValidationError> Validate(object instance);
    }
}