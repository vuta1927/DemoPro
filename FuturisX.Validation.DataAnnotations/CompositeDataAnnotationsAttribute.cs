using System;
using System.Collections.Generic;

namespace FuturisX.Validation.DataAnnotations
{
    public abstract class CompositeDataAnnotationsAttribute : Attribute
    {
        public abstract IEnumerable<Attribute> GetAttributes();
    }
}