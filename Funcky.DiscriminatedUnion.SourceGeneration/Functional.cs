namespace Funcky.DiscriminatedUnion.SourceGeneration;

internal static class Functional
{
    public static Lazy<T> Lazy<T>(Func<T> func) => new(func);
}
