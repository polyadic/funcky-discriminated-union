namespace Foo
{
    namespace Bar
    {
        [Funcky.DiscriminatedUnion]
        public abstract partial record NestedUnion
        {
            public sealed partial record Variant : NestedUnion;
        }
    }
}
