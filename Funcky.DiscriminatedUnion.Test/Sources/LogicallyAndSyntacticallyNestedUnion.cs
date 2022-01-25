using System;

namespace Funcky.DiscriminatedUnion.Test;

[DiscriminatedUnion]
public abstract partial record SyntaxNode
{
    public sealed partial record Keyword(string Value) : SyntaxNode;

    [DiscriminatedUnion]
    public abstract partial record Literal : SyntaxNode
    {
        public sealed partial record Integer(int Value) : Literal;

        public sealed partial record String(string Value) : Literal;
    }
}

public static class SyntaxNodeTest
{
    public static void SyntaxNodeFn(SyntaxNode syntaxNode)
    {
        _ = syntaxNode.Match(
            keyword: keyword => keyword.Value,
            literal: literal => literal.Match(
                integer: integer => integer.Value.ToString(),
                @string: @string => @string.Value));

        syntaxNode.Switch(
            keyword: keyword => Console.WriteLine(keyword.Value),
            literal: literal => literal.Switch(
                integer: integer => Console.WriteLine(integer.Value.ToString()),
                @string: @string => Console.WriteLine(@string.Value)));
    }
}
