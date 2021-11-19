# Funcky Discriminated Unions
A source generator that generates `Match` methods for all your discriminated unions needs. ✨

## Usage
Apply the `[DiscriminatedUnion]` to an abstract class (or record) with nested types representing the variants.

## Example
```cs
using Funcky;

var result = Result<int>.Ok(42);
var resultOrFallback = result.Match(ok: ok => ok.Value, error: _ => 0);

[DiscriminatedUnion]
public abstract partial record Result<T>
    where T : notnull
{
    public sealed partial record Ok(T Value) : Result<T>;

    public sealed partial record Error(Exception Exception) : Result<T>;
}
```

## Settings
The attribute allows configuration of some aspects of source generation.

### `NonExhaustive`
The auto-generated `Match` and `Switch` methods are public by default.
When `NonExhaustive` is set to `true`, these methods are generated with `internal` visibility instead.

### `Flatten`
The auto-generated `Match` and `Switch` methods only accept one level of inheritance by default.
Set `Flatten` to `true` to include arbitrarily deep inherited types in these methods.

```cs
using Funcky;

SyntaxNode node = ...;
var nodeAsString = node.Match(
    keyword: keyword => keyword.Value,
    integer: integer => integer.Value.ToString(),
    double: @double => @double.Value.ToString());

[DiscriminatedUnion(Flatten = true)]
public abstract partial record SyntaxNode
{
    public sealed partial record Keyword(string Value) : SyntaxNode;

    public abstract partial record Literal : SyntaxNode;

    public abstract partial record Number : Literal;

    public sealed partial record Integer(int Value) : Number;

    public sealed partial record Double(double Value) : Number;
}
```
