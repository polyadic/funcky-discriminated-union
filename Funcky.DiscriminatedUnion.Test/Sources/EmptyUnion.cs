[Funcky.DiscriminatedUnion]
public abstract partial record EmptyUnion
{
}

public static class EmptyUnionTest
{
    public static void EmptyUnionFn(EmptyUnion empty)
    {
        _ = empty.Match<string>();
        empty.Switch();
    }
}
