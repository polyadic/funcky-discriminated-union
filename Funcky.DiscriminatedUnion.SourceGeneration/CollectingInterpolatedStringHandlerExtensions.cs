namespace Funcky.DiscriminatedUnion.SourceGeneration;

public static class CollectingInterpolatedStringHandlerExtensions
{
    public static CollectingInterpolatedStringHandler JoinToInterpolation<T>(
        this IEnumerable<T> source,
        string separator)
    {
        using var enumerator = source.GetEnumerator();

        if (!enumerator.MoveNext())
        {
            return new CollectingInterpolatedStringHandler();
        }

        var result = new CollectingInterpolatedStringHandler();

        result.AppendFormatted(enumerator.Current);

        while (enumerator.MoveNext())
        {
            result.AppendLiteral(separator);
            result.AppendFormatted(enumerator.Current);
        }

        return result;
    }

    public static CollectingInterpolatedStringHandler JoinToInterpolation<T>(
        this IEnumerable<T> source,
        Func<T, CollectingInterpolatedStringHandler> createPart,
        string separator)
    {
        using var enumerator = source.GetEnumerator();

        if (!enumerator.MoveNext())
        {
            return new CollectingInterpolatedStringHandler();
        }

        var result = new CollectingInterpolatedStringHandler();

        result.AppendFormatted(createPart(enumerator.Current));

        while (enumerator.MoveNext())
        {
            result.AppendLiteral(separator);
            result.AppendFormatted(createPart(enumerator.Current));
        }

        return result;
    }
}
