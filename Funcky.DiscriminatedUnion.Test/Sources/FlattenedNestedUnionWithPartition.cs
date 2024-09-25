namespace Funcky.DiscriminatedUnion.Test;

[DiscriminatedUnion(Flatten = true, GeneratePartitionExtension = true)]
public abstract partial record FlattenedNestedUnionWithPartition
{
    public sealed partial record Keyword(string Value) : FlattenedNestedUnionWithPartition;

    public abstract partial record Literal : FlattenedNestedUnionWithPartition
    {
        public abstract partial record Number : Literal
        {
            public sealed partial record Integer(int Value) : Number;
        }
    }
}

public static class FlattenedNestedUnionWithPartitionTest
{
    public static void Test(FlattenedNestedUnionWithPartition[] items)
    {
        var (keywords, integers) = items.Partition();
    }
}
