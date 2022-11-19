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

[Funcky.DiscriminatedUnion]
[System.Text.Json.Serialization.JsonDerivedType(typeof(Rectangle), typeDiscriminator: 1)]
[System.Text.Json.Serialization.JsonDerivedType(typeof(Circle), typeDiscriminator: "⏺")]
public abstract partial record Shape
{
    public sealed partial record Rectangle(double Width, double Length) : Shape;

    public sealed partial record Circle(double Radius) : Shape;

    public partial record EquilateralTriangle(double SideLength) : Shape;
}
