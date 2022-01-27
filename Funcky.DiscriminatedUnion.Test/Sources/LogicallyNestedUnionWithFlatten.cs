namespace Funcky.DiscriminatedUnion.Test;

[DiscriminatedUnion(Flatten = true)]
public abstract partial record SyntaxNodeFlattened
{
    public sealed partial record Keyword(string Value) : SyntaxNodeFlattened;

    public abstract partial record Literal : SyntaxNodeFlattened;

    public abstract partial record Number : Literal;

    public sealed partial record Integer(int Value) : Number;
}

public static class SyntaxNodeFlattenedTest
{
    public static void SyntaxNodeFn(SyntaxNodeFlattened syntaxNode)
    {
        _ = syntaxNode.Match(
            keyword: keyword => keyword.Value,
            integer: integer => integer.Value.ToString());
    }
}
