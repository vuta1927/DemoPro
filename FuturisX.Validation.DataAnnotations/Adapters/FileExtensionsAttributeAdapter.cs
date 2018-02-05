using System.ComponentModel.DataAnnotations;

namespace FuturisX.Validation.DataAnnotations.Adapters
{
    public class FileExtensionsAttributeAdapter : DataAnnotationsValidatorAdapter
    {
        public override bool CanHandle(ValidationAttribute attribute)
        {
            return attribute is FileExtensionsAttribute;
        }
    }
}