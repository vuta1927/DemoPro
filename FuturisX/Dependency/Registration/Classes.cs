﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using FuturisX.Helpers.Reflection;

namespace FuturisX.Dependency.Registration
{
    public static class Classes
    {
        /// <summary>Prepares to register types from a list of types.</summary>
		/// <param name = "types"> The list of types. </param>
		/// <returns>
		///     The corresponding <see cref = "FromDescriptor" />
		/// </returns>
		public static FromTypesDescriptor From(IEnumerable<Type> types)
        {
            return new FromTypesDescriptor(types, Filter);
        }

        /// <summary>Prepares to register types from a list of types.</summary>
        /// <param name = "types"> The list of types. </param>
        /// <returns>
        ///     The corresponding <see cref = "FromDescriptor" />
        /// </returns>
        public static FromTypesDescriptor From(params Type[] types)
        {
            return new FromTypesDescriptor(types, Filter);
        }

        /// <summary>Prepares to register types from an assembly.</summary>
        /// <param name = "assembly"> The assembly. </param>
        /// <returns>
        ///     The corresponding <see cref = "FromDescriptor" />
        /// </returns>
        public static FromAssemblyDescriptor FromAssembly(Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }
            return new FromAssemblyDescriptor(assembly, Filter);
        }

        /// <summary>Prepares to register types from an assembly containing the type.</summary>
        /// <param name = "type"> The type belonging to the assembly. </param>
        /// <returns>
        ///     The corresponding <see cref = "FromDescriptor" />
        /// </returns>
        public static FromAssemblyDescriptor FromAssemblyContaining(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            return new FromAssemblyDescriptor(type.GetTypeInfo().Assembly, Filter);
        }

        /// <summary>Prepares to register types from an assembly containing the type.</summary>
        /// <typeparam name = "T"> The type belonging to the assembly. </typeparam>
        /// <returns>
        ///     The corresponding <see cref = "FromDescriptor" />
        /// </returns>
        public static FromAssemblyDescriptor FromAssemblyContaining<T>()
        {
            return FromAssemblyContaining(typeof(T));
        }

        /// <summary>Prepares to register types from assemblies found in a given directory that meet additional optional restrictions.</summary>
        /// <param name = "filter"> </param>
        /// <returns> </returns>
        public static FromAssemblyDescriptor FromAssemblyInDirectory(AssemblyFilter filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }
            var assemblies = ReflectionUtil.GetAssemblies(filter);
            return new FromAssemblyDescriptor(assemblies, Filter);
        }

        /// <summary>Scans current assembly and all refernced assemblies with the same first part of the name.</summary>
        /// <returns> </returns>
        /// <remarks>
        ///     Assemblies are considered to belong to the same application based on the first part of the name. For example if the method is called from within <c>MyApp.exe</c> and <c>MyApp.exe</c> references
        ///     <c>MyApp.SuperFeatures.dll</c>, <c>mscorlib.dll</c> and <c>ThirdPartyCompany.UberControls.dll</c> the <c>MyApp.exe</c> and <c>MyApp.SuperFeatures.dll</c> will be scanned for components, and other
        ///     assemblies will be ignored.
        /// </remarks>

        public static FromAssemblyDescriptor FromAssemblyInThisApplication(Assembly rootAssembly)
        {
            var assemblies = new HashSet<Assembly>(ReflectionUtil.GetApplicationAssemblies(rootAssembly));
            return new FromAssemblyDescriptor(assemblies, Filter);
        }

        /// <summary>Prepares to register types from an assembly.</summary>
        /// <param name = "assemblyName"> The assembly name. </param>
        /// <returns>
        ///     The corresponding <see cref = "FromDescriptor" />
        /// </returns>
        public static FromAssemblyDescriptor FromAssemblyNamed(string assemblyName)
        {
            var assembly = ReflectionUtil.GetAssemblyNamed(assemblyName);
            return FromAssembly(assembly);
        }

        /// <summary>Prepares to register types from the assembly containing the code invoking this method.</summary>
        /// <returns>
        ///     The corresponding <see cref = "FromDescriptor" />
        /// </returns>

        [EditorBrowsable(EditorBrowsableState.Never)]
        internal static bool Filter(Type type)
        {
            return type.GetTypeInfo().IsClass && type.GetTypeInfo().IsAbstract == false;
        }
    }
}