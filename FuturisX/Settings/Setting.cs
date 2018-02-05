using System;
using FuturisX.Domain.Entities;

namespace FuturisX.Settings
{
    public class Setting : Entity<Guid>
    {
//        [Required, ShortString]
        public string Category { get; set; }
//        [Required, ShortString]
        public string Name { get; set; }
        public string Value { get; set; }
    }
}