using System.Reflection.Emit;
using FuturisX.Mapping.Runtime;

namespace FuturisX.Mapping.Mappers.Creator
{
    internal interface IInstanceCreator<TTarget>
    {
        void Compile(ModuleBuilder builder);

        void Emit(CompilationContext context);
    }
}
