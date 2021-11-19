namespace Funcky.DiscriminatedUnion.SourceGeneration;

internal sealed class SelectorComparer<TSource, TSelected> : IEqualityComparer<TSource>
{
    private readonly Func<TSource, TSelected> _selector;

    public SelectorComparer(Func<TSource, TSelected> selector) => _selector = selector;

    public bool Equals(TSource x, TSource y) => EqualityComparer<TSelected>.Default.Equals(_selector(x), _selector(y));

    public int GetHashCode(TSource obj) => EqualityComparer<TSelected>.Default.GetHashCode(_selector(obj));
}