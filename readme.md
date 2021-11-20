# Funcky Discriminated Unions
A source generator that generates `Match` methods for all your discriminated unions needs. ✨

[![Build](https://github.com/polyadic/funcky-discriminated-union/workflows/Build/badge.svg)](https://github.com/polyadic/funcky-discriminated-union/actions?query=workflow%3ABuild)
[![Licence: MIT](https://img.shields.io/badge/licence-MIT-green)](https://raw.githubusercontent.com/polyadic/funcky-discriminated-union/main/license-mit.txt)
[![Licence: Apache](https://img.shields.io/badge/licence-Apache-green)](https://raw.githubusercontent.com/polyadic/funcky-discriminated-union/main/license-apache.txt)

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

## Minimum Required Versions
* Visual Studio 2022
* Roslyn 4.0.0
* .NET 6

## Settings
The attribute allows configuration of some aspects of source generation.

### `NonExhaustive`
The auto-generated `Match` and `Switch` methods are public by default.
When `NonExhaustive` is set to `true`, these methods are generated with `internal` visibility instead.

### `MatchResultTypeName`
The auto-generated `Match` method uses a generic type for the result. This type is named `TResult` by default.
This can cause conflict with generic types on the discriminated union itself. Use `MatchResultTypeName` to set a custom name for this type.

```cs
using Funcky;

[DiscriminatedUnion(MatchResultTypeName = "TMatchResult")]
public abstract partial record Result<TResult> { ... }

// Generated code
partial record Result<TResult>
{
    public abstract TMatchResult Match<TMatchResult>(...);

    ...
}
```

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

## License
This work is dual-licensed under MIT and Apache 2.0.
You can choose between one of them if you use this work.

`SPDX-License-Identifier: MIT OR Apache 2.0`
