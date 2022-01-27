using System.Runtime.CompilerServices;

namespace Funcky.DiscriminatedUnion.Test;

public static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Init()
    {
        VerifySourceGenerators.Enable();
    }
}
