using System.ComponentModel.DataAnnotations;

namespace FuturisX.Validation.DataAnnotations.Adapters
{
    public class RegularExpressionAttributeAdapter : DataAnnotationsValidatorAdapter
    {
        public override bool CanHandle(ValidationAttribute attribute)
        {
            return attribute is RegularExpressionAttribute;
        }
    }
}