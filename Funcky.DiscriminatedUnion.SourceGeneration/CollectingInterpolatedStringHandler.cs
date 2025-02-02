using System.Runtime.CompilerServices;

namespace Funcky.DiscriminatedUnion.SourceGeneration;

[InterpolatedStringHandler]
public readonly struct CollectingInterpolatedStringHandler
{
    private readonly List<object?> _items;

    public CollectingInterpolatedStringHandler(int literalLength, int formattedCount)
    {
        _items = new List<object?>(formattedCount * 2);
    }

    public CollectingInterpolatedStringHandler()
    {
        _items = [];
    }

    public IEnumerable<object?> GetItems() => _items;

    public void AppendLiteral(string s) => _items.Add(s);

    public void AppendFormatted<T>(T t) => _items.Add(t);

    public void AppendFormatted(CollectingInterpolatedStringHandler handler)
        => _items.AddRange(handler._items);
}
