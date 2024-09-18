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
    public static void Test()
    {
        var instances = new UnionWithPartitionUsage[]
        {
            new UnionWithPartitionUsage.Success(),
            new UnionWithPartitionUsage.Warning(),
            new UnionWithPartitionUsage.Error(),
            new UnionWithPartitionUsage.Success(),
            new UnionWithPartitionUsage.Warning(),
            new UnionWithPartitionUsage.Warning(),
        };

        var (successes, warnings, errors) = instances.Partition();
    }
}
