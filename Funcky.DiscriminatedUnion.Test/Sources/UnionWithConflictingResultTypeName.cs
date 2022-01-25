[Funcky.DiscriminatedUnion(MatchResultTypeName = "TMatchResult")]
public abstract partial record UnionWithConflictingGenericType<TResult>
{
    public sealed partial record Variant : UnionWithConflictingGenericType<TResult>;
}
