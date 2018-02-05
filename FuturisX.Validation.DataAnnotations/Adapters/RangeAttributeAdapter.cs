using System.ComponentModel.DataAnnotations;

namespace FuturisX.Validation.DataAnnotations.Adapters
{
    /// <summary>
    /// An adapter for the <see cref="RangeAttribute"/>.
    /// </summary>
    public class RangeAttributeAdapter : DataAnnotationsValidatorAdapter
    {
        public override bool CanHandle(ValidationAttribute attribute)
        {
            return attribute is RangeAttribute;
        }
    }
}