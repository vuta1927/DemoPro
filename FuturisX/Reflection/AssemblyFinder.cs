using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FuturisX.Helpers.Extensions;

namespace FuturisX.Reflection
{
    public class AssemblyFinder : IAssemblyFinder
    {
        public List<Assembly> GetAllAssemblies()
        {
            return AppDomain.CurrentDomain.GetExcutingAssembiles().ToList();
        }
    }
}