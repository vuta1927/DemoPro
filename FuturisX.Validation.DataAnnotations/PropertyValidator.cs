using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FuturisX.Validation.DataAnnotations
{
    public class PropertyValidator : IPropertyValidator
    {
        public IDictionary<ValidationAttribute, IEnumerable<IDataAnnotationsValidatorAdapter>> AttributeAdaptors { get; set; }
        
        public PropertyDescriptor Descriptor { get; set; }
        
        public IEnumerable<ValidationError> Validate(object instance)
        {
            var errors = new List<ValidationError>();

            foreach (var attributeAdapter in AttributeAdaptors)
            {
                foreach (var adapter in attributeAdapter.Value)
                {
                    var results = adapter.Validate(instance, attributeAdapter.Key, Descriptor);
                    errors.AddRange(results);
                }
            }

            return errors;
        }
    }
}