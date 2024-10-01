namespace Funcky.DiscriminatedUnion.Test;

[DiscriminatedUnion(Flatten = true, GeneratePartitionExtension = true)]
public abstract partial record FlattenedUnionWithPartition
{
    public sealed partial record Keyword(string Value) : FlattenedUnionWithPartition;

    public abstract partial record Literal : FlattenedUnionWithPartition;

    public abstract partial record Number : Literal;

    public sealed partial record Integer(int Value) : Number;
}

public static class FlattenedUnionWithPartitionTest
{
    public static void Test(FlattenedUnionWithPartition[] items)
    {
        var (keywords, integers) = items.Partition();
    }
}
