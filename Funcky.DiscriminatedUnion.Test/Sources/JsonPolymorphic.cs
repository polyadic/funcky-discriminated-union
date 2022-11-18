[Funcky.DiscriminatedUnion]
[System.Text.Json.Serialization.JsonPolymorphic]
public abstract partial record Result
{
    public sealed partial record Ok(int Value) : Result;

    public sealed partial record Error(string Message) : Result;
}
