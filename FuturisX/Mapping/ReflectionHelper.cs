﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace FuturisX.Mapping
{
    internal static class ReflectionHelper
    {
        public static TypeBuilder DefineStaticType(this ModuleBuilder builder)
        {
            return builder.DefineType(Guid.NewGuid().ToString("N"),
                TypeAttributes.Public | TypeAttributes.Abstract | TypeAttributes.Sealed | TypeAttributes.AutoClass |
                TypeAttributes.AnsiClass | TypeAttributes.BeforeFieldInit);
        }

        public static MethodBuilder DefineStaticMethod(this TypeBuilder builder, string methodName)
        {
            return builder.DefineMethod(methodName, MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.HideBySig);
        }

        public static FieldBuilder DefineStaticField<T>(this TypeBuilder builder, string fieldName)
        {
            return builder.DefineField(fieldName, typeof(T), FieldAttributes.Public | FieldAttributes.Static);
        }

        public static bool IsNullable(this Type type)
        {
            var typeInfo = type.GetTypeInfo();
            return typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        public static MethodInfo GetConvertMethod(Type sourceType, Type targetType)
        {
            if (sourceType == null || targetType == null) return null;

            var reflectingSourceType = sourceType.GetTypeInfo();
            var reflectingTargetType = targetType.GetTypeInfo();

            bool MethodPredicate(MethodInfo method, string name)
            {
                if (method.IsSpecialName && method.Name == name && method.ReturnType == targetType)
                {
                    var parameters = method.GetParameters();
                    return parameters.Length == 1 && parameters[0].ParameterType == sourceType;
                }
                return false;
            }

            return
                reflectingTargetType.GetMethods(BindingFlags.Public | BindingFlags.Static).FirstOrDefault(method => MethodPredicate(method, "op_Implicit")) ??
                reflectingSourceType.GetMethods(BindingFlags.Public | BindingFlags.Static).FirstOrDefault(method => MethodPredicate(method, "op_Implicit")) ??
                reflectingTargetType.GetMethods(BindingFlags.Public | BindingFlags.Static).FirstOrDefault(method => MethodPredicate(method, "op_Explicit")) ??
                reflectingSourceType.GetMethods(BindingFlags.Public | BindingFlags.Static).FirstOrDefault(method => MethodPredicate(method, "op_Explicit"));
        }

        public static int GetDistance(Type sourceType, Type targetType)
        {
            if (targetType == null) return -1;
            var reflectingSourceType = sourceType?.GetTypeInfo();
            var reflectingTargetType = targetType.GetTypeInfo();
            if (reflectingSourceType != null && reflectingTargetType.IsInterface)
            {
                return reflectingSourceType.GetInterfaces().Count(interfaceType =>
                {
                    var reflectingInterfaceType = interfaceType.GetTypeInfo();
                    if (!reflectingTargetType.IsGenericTypeDefinition)
                    {
                        return reflectingTargetType.IsAssignableFrom(reflectingInterfaceType);
                    }
                    if (reflectingInterfaceType.IsGenericType)
                    {
                        return reflectingTargetType.IsAssignableFrom(reflectingInterfaceType.GetGenericTypeDefinition());
                    }
                    if (reflectingInterfaceType.IsGenericTypeDefinition)
                    {
                        return reflectingTargetType.IsAssignableFrom(reflectingInterfaceType);
                    }
                    return false;
                });
            }
            var distance = 0;
            while (reflectingSourceType != null)
            {
                if (reflectingSourceType.AsType() == reflectingTargetType.AsType()) return distance;
                if (reflectingSourceType.AsType() == typeof(object)) break;
                reflectingSourceType = reflectingSourceType.BaseType?.GetTypeInfo();
                distance++;
            }
            return -1;
        }

        public static bool IsEnumerable(this Type targetType, out Type elementType)
        {
            var reflectingTargetType = targetType.GetTypeInfo();
            elementType = null;
            IEnumerable<Type> interfaces = reflectingTargetType.GetInterfaces();
            if (reflectingTargetType.IsInterface)
            {
                interfaces = interfaces.Concat(new[] { targetType });
            }
            var matchedType = interfaces.FirstOrDefault(type =>
            {
                if (type == typeof(IEnumerable<>)) return true;
                var reflectingType = type.GetTypeInfo();
                return reflectingType.IsGenericType && reflectingType.GetGenericTypeDefinition() == typeof(IEnumerable<>);
            });
            if (matchedType != null)
            {
                elementType = matchedType.GetTypeInfo().GetGenericArguments()[0];
                return true;
            }
            return false;
        }

        public static int CombineHashCodes(int h1, int h2)
        {
            return ((h1 << 5) + h1) ^ h2;
        }

        public static int CombineHashCodes(int h1, int h2, int h3)
        {
            return CombineHashCodes(CombineHashCodes(h1, h2), h3);
        }
    }
}
