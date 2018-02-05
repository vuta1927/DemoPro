using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FuturisX.Validation.DataAnnotations
{
    /// <summary>
    /// Defines the functionality for an adapter for models that implements the <see cref="IValidatableObject"/> interface.
    /// </summary>
    public interface IValidatableObjectAdapter
    {
        /// <summary>
        /// Validates the given instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="ValidationError"/> instances.</returns>
        IEnumerable<ValidationError> Validate(object instance);
    }
}