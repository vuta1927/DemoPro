using System.Reflection.Emit;

namespace FuturisX.Mapping.Runtime
{
    internal interface IInvokerBuilder
    {
        void Compile(ModuleBuilder builder);

        void Emit(CompilationContext context);
    }
}
