using System;

public partial interface IInterface
{
    public partial record class RecordClass
    {
        public partial record Record
        {
            public partial class Class
            {
                public static partial class StaticClass
                {
                    [Funcky.DiscriminatedUnion]
                    public abstract partial record NestedUnion
                    {
                        public sealed partial record Variant : NestedUnion;
                    }
                }
            }
        }
    }
}

public static class NestedUnionTest
{
    public static void NestedUnionTestFn(IInterface.RecordClass.Record.Class.StaticClass.NestedUnion nested)
    {
        nested.Switch(variant: v => Console.WriteLine(v));
    }
}
