namespace Funcky.DiscriminatedUnion.Test.Sources;

[DiscriminatedUnion(GeneratePartitionExtension = true)]
public abstract partial record UnionWithPartitionUsage
{
    public sealed partial record Success : UnionWithPartitionUsage;

    public sealed partial record Warning : UnionWithPartitionUsage;

    public sealed partial record Error : UnionWithPartitionUsage;
}

public static class UnionWithPartitionUsageTest
{
    public static void Test(UnionWithPartitionUsage[] items)
    {
        var (successes, warnings, errors) = items.Partition();

        int _ = items.Partition(resultSelector: (_, w, e) => w.Count + e.Count);
    }
}
