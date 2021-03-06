﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using FuturisX.Helpers.Exception;
using FuturisX.Mapping.Conventions;
using FuturisX.Mapping.Mappers.Creator;
using FuturisX.Mapping.Mappers.MemberMapper;
using FuturisX.Mapping.Runtime;

namespace FuturisX.Mapping.Mappers.TypeMapper
{
    internal class TypeMapper<TSource, TTarget> : ITypeMapper<TSource, TTarget>
    {
        private readonly MappingContainer _container;
        private IInstanceCreator<TTarget> _creator;
        private Action<TSource, TTarget> _beforeMapAction;
        private Action<TSource, TTarget> _customMapper;
        private Action<TSource, TTarget> _afterMapAction;
        private ActionInvokerBuilder<TSource, TTarget> _beforeMapBuilder;
        private ActionInvokerBuilder<TSource, TTarget> _customInvokerBuilder;
        private ActionInvokerBuilder<TSource, TTarget> _afterMapBuilder;
        private MemberMapperCollection _memberMappers;
        private MappingMemberCollection _targetMembers;
        private MemberMapOptions _options;
        private bool _readonly;
        private bool _compiled;
        private bool _initialized;
        private readonly object _lockObj = new object();

        private static readonly ConcurrentDictionary<MappingContainer, TypeMapper<TSource, TTarget>> _instances =
            new ConcurrentDictionary<MappingContainer, TypeMapper<TSource, TTarget>>();

        public static TypeMapper<TSource, TTarget> GetInstance(MappingContainer container)
        {
            return _instances.GetOrAdd(container, key => new TypeMapper<TSource, TTarget>(key));
        }

        private TypeMapper(MappingContainer container)
        {
            _container = container;
        }

        private void Initialize()
        {
            if (!_initialized)
            {
                lock (_lockObj)
                {
                    if (!_initialized)
                    {
                        var context = new ConventionContext(_container, typeof(TSource), typeof(TTarget), _options);
                        _container.Conventions.Apply(context);
                        _targetMembers = context.TargetMembers;
                        _memberMappers = new MemberMapperCollection(_container, _options);
                        foreach (var mapping in context.Mappings)
                        {
                            _memberMappers.Set(mapping.TargetMember, mapping.SourceMember, mapping.Converter);
                        }
                        _creator = context.Creator != null
                            ? (IInstanceCreator<TTarget>) new ConventionCreator<TTarget>(context.Creator)
                            : new DefaultCreator<TTarget>();
                        _initialized = true;
                    }
                }
            }
        }

        private void CheckReadOnly()
        {
            if (_readonly)
            {
                throw new NotSupportedException("The type mapper is read-only.");
            }
        }

        public void SetReadOnly()
        {
            if (!_readonly)
            {
                _readonly = true;
            }
        }

        public void Compile(ModuleBuilder builder)
        {
            if (!_compiled)
            {
                Initialize();
                _creator.Compile(builder);
                if (_beforeMapAction != null)
                {
                    _beforeMapBuilder = new ActionInvokerBuilder<TSource, TTarget>(_beforeMapAction);
                    _beforeMapBuilder.Compile(builder);
                }
                if (_customMapper != null)
                {
                    _customInvokerBuilder = new ActionInvokerBuilder<TSource, TTarget>(_customMapper);
                    _customInvokerBuilder.Compile(builder);
                }
                else
                {
                    foreach (var mapper in _memberMappers)
                    {
                        mapper.Compile(builder);
                    }
                }
                if (_afterMapAction != null)
                {
                    _afterMapBuilder = new ActionInvokerBuilder<TSource, TTarget>(_afterMapAction);
                    _afterMapBuilder.Compile(builder);
                }
                _compiled = true;
            }
        }

        private void EmitMap(CompilationContext context)
        {
            if (_beforeMapBuilder != null)
            {
                context.LoadSource(LoadPurpose.Parameter);
                context.LoadTarget(LoadPurpose.Parameter);
                _beforeMapBuilder.Emit(context);
            }
            if (_customInvokerBuilder != null)
            {
                context.LoadSource(LoadPurpose.Parameter);
                context.LoadTarget(LoadPurpose.Parameter);
                _customInvokerBuilder.Emit(context);
            }
            else
            {
                foreach (var mapper in _memberMappers)
                {
                    mapper.Emit(context);
                }
            }
            if (_afterMapBuilder != null)
            {
                context.LoadSource(LoadPurpose.Parameter);
                context.LoadTarget(LoadPurpose.Parameter);
                _afterMapBuilder.Emit(context);
            }
        }

        public Action<TSource, TTarget> CreateMapper(ModuleBuilder builder)
        {
            Initialize();
            if (_customMapper != null) return _customMapper;
            var typeBuilder = builder.DefineStaticType();
            var methodBuilder = typeBuilder.DefineStaticMethod("Map");
            methodBuilder.SetReturnType(typeof(void));
            methodBuilder.SetParameters(typeof(TSource), typeof(TTarget));

            var reflectingTargetType = typeof(TTarget).GetTypeInfo();
            var reflectingSourceType = typeof(TSource).GetTypeInfo();

            var il = methodBuilder.GetILGenerator();
            var context = new CompilationContext(il);
            if (reflectingSourceType.IsValueType)
            {
                context.SetSource(purpose =>
                {
                    if (purpose == LoadPurpose.MemberAccess)
                    {
                        il.Emit(OpCodes.Ldarga_S, 0);
                    }
                    else
                    {
                        il.Emit(OpCodes.Ldarg_0);
                    }
                });
            }
            else
            {
                context.SetSource(purpose => il.Emit(OpCodes.Ldarg_0));
            }
            if (reflectingTargetType.IsValueType)
            {
                context.SetTarget(purpose =>
                {
                    if (purpose == LoadPurpose.MemberAccess)
                    {
                        il.Emit(OpCodes.Ldarga_S, 1);
                    }
                    else
                    {
                        il.Emit(OpCodes.Ldarg_1);
                    }
                });
            }
            else
            {
                context.SetTarget(purpose => il.Emit(OpCodes.Ldarg_1));
            }
            EmitMap(context);
            context.Emit(OpCodes.Ret);
            return (Action<TSource, TTarget>)typeBuilder.CreateTypeInfo().GetMethod("Map", BindingFlags.Static | BindingFlags.Public).CreateDelegate(typeof(Action<TSource, TTarget>));
        }

        public Func<TSource, TTarget> CreateConverter(ModuleBuilder builder)
        {
            Initialize();
            var typeBuilder = builder.DefineStaticType();
            var methodBuilder = typeBuilder.DefineStaticMethod("Map");
            methodBuilder.SetReturnType(typeof(TTarget));
            methodBuilder.SetParameters(typeof(TSource));

            var reflectingTargetType = typeof(TTarget).GetTypeInfo();
            var reflectingSourceType = typeof(TSource).GetTypeInfo();

            var il = methodBuilder.GetILGenerator();
            var context = new CompilationContext(il);
            if (reflectingSourceType.IsValueType)
            {
                context.SetSource(purpose =>
                {
                    if (purpose == LoadPurpose.MemberAccess)
                    {
                        il.Emit(OpCodes.Ldarga_S, 0);
                    }
                    else
                    {
                        il.Emit(OpCodes.Ldarg_0);
                    }
                });
            }
            else
            {
                context.SetSource(purpose => il.Emit(OpCodes.Ldarg_0));
            }
            var targetLocal = il.DeclareLocal(typeof(TTarget));
            _creator.Emit(context);
            il.Emit(OpCodes.Stloc, targetLocal);
            if (reflectingTargetType.IsValueType)
            {
                context.SetTarget(
                    purpose =>
                        il.Emit(purpose == LoadPurpose.MemberAccess ? OpCodes.Ldloca_S : OpCodes.Ldloc, targetLocal));
            }
            else
            {
                context.SetTarget(purpose => il.Emit(OpCodes.Ldloc, targetLocal));
            }
            EmitMap(context);
            context.LoadTarget(LoadPurpose.ReturnValue);
            context.Emit(OpCodes.Ret);
            return (Func<TSource, TTarget>)typeBuilder.CreateTypeInfo().GetMethod("Map", BindingFlags.Static | BindingFlags.Public).CreateDelegate(typeof(Func<TSource, TTarget>));
        }

        #region Configuration

        public ITypeMapper<TSource, TTarget> WithOptions(MemberMapOptions options)
        {
            if (_initialized)
            {
                throw new NotSupportedException("The type mapper has been initialized. Please configure options before the other configurations.");
            }
            _options = options;
            return this;
        }

        public ITypeMapper<TSource, TTarget> MapWith(Action<TSource, TTarget> expression)
        {
            CheckReadOnly();
            Throw.IfArgumentNull(expression, nameof(expression));
            _customMapper = expression;
            return this;
        }

        public ITypeMapper<TSource, TTarget> CreateWith(Func<TSource, TTarget> expression)
        {
            CheckReadOnly();
            Initialize();
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }
            _creator = new LambdaCreator<TSource, TTarget>(expression);
            return this;
        }

        public ITypeMapper<TSource, TTarget> MapMember<TMember>(string targetName, Func<TSource, TMember> expression)
        {
            CheckReadOnly();
            Initialize();
            if (string.IsNullOrEmpty(targetName))
            {
                throw new ArgumentException("The name of the target member cannot be null or empty.", nameof(targetName));
            }
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }
            var targetMember = _targetMembers[targetName];
            if (targetMember != null)
            {
                _memberMappers.Set(targetMember, expression);
            }
            return this;
        }

        public ITypeMapper<TSource, TTarget> Ignore(IEnumerable<string> members)
        {
            CheckReadOnly();
            Initialize();
            if (members == null)
            {
                throw new ArgumentNullException(nameof(members));
            }
            foreach (var member in members)
            {
                if (string.IsNullOrEmpty(member))
                {
                    throw new ArgumentException("The name of the target member cannot be null or empty.");
                }
                var targetMember = _targetMembers[member];
                if (targetMember != null)
                {
                    _memberMappers.Remove(targetMember);
                }
            }
            return this;
        }

        public ITypeMapper<TSource, TTarget> BeforeMap(Action<TSource, TTarget> expression)
        {
            CheckReadOnly();
            Throw.IfArgumentNull(expression, nameof(expression));
            _beforeMapAction = expression;
            return this;
        }

        public ITypeMapper<TSource, TTarget> AfterMap(Action<TSource, TTarget> expression)
        {
            CheckReadOnly();
            Throw.IfArgumentNull(expression, nameof(expression));
            _afterMapAction = expression;
            return this;
        }

        #endregion
    }
}
