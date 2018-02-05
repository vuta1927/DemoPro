using System.ComponentModel.DataAnnotations;

namespace FuturisX.Validation.DataAnnotations.Adapters
{
    public class RequiredAttributeAdapter : DataAnnotationsValidatorAdapter
    {
        public override bool CanHandle(ValidationAttribute attribute)
        {
            return attribute is RequiredAttribute;
        }
    }
}