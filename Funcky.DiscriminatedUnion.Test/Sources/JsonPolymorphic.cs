[Funcky.DiscriminatedUnion]
[System.Text.Json.Serialization.JsonPolymorphic]
public abstract partial record Result
{
    public sealed partial record Ok(int Value) : Result;

    public sealed partial record Error(string Message) : Result;
}

public partial class Nesting1<A, B, C>
{
    public partial class Nesting2<D>
    {
        [Funcky.DiscriminatedUnion]
        [System.Text.Json.Serialization.JsonPolymorphic]
        public abstract partial record Result<T>
        {
            public sealed partial record Ok(int Value) : Result<T>;

            public sealed partial record Error(string Message) : Result<T>;
        }
    }
}
