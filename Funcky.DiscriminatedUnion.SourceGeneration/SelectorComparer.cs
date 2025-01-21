namespace Funcky.DiscriminatedUnion.SourceGeneration;

internal sealed class SelectorComparer<TSource, TSelected>(Func<TSource, TSelected> selector)
    : IEqualityComparer<TSource>
{
    public bool Equals(TSource x, TSource y) => EqualityComparer<TSelected>.Default.Equals(selector(x), selector(y));

    public int GetHashCode(TSource obj) => EqualityComparer<TSelected>.Default.GetHashCode(selector(obj));
}
