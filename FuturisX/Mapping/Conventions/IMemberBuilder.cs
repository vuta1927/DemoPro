﻿using FuturisX.Mapping.Runtime;

namespace FuturisX.Mapping.Conventions
{
    internal interface IMemberBuilder
    {
        void EmitGetter(CompilationContext context);

        void EmitSetter(CompilationContext context);
    }
}
