namespace Funcky.DiscriminatedUnion.Playground
{
    [DiscriminatedUnion(NonExhaustive = true)]
    public abstract partial record Result<T>
        where T : notnull
    {
        public sealed partial record Ok(T Result) : Result<T>;

        public sealed partial record Error(Exception Exception) : Result<T>;
    }

    public static class ResultTest
    {
        public static void ResultFn(Result<int> result)
        {
            _ = result.Match(ok: ok => ok.Result, error: _ => 0);
        }
    }

    [DiscriminatedUnion]
    public abstract partial record Bool
    {
        public sealed partial record True : Bool;

        public sealed partial record False : Bool;
    }

    public static class BoolTest
    {
        public static void BoolFn(Bool @bool)
        {
            _ = @bool.Match(@true: _ => true, @false: _ => false);
        }
    }

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

    [DiscriminatedUnion(Flatten = true)]
    public abstract partial record SyntaxNodeFlattened
    {
        public sealed partial record Keyword(string Value) : SyntaxNodeFlattened;

        public abstract partial record Literal : SyntaxNodeFlattened
        {
            public abstract partial record Number : Literal
            {
                public sealed partial record Integer(int Value) : Number;
            }
        }
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

    [DiscriminatedUnion(Flatten = true)]
    public abstract partial record SyntaxNodeFlattened2
    {
        public sealed partial record Keyword(string Value) : SyntaxNodeFlattened2;

        public abstract partial record Literal : SyntaxNodeFlattened2;

        public abstract partial record Number : Literal;

        public sealed partial record Integer(int Value) : Number;
    }

    public static class SyntaxNodeFlattened2Test
    {
        public static void SyntaxNodeFn(SyntaxNodeFlattened2 syntaxNode)
        {
            _ = syntaxNode.Match(
                keyword: keyword => keyword.Value,
                integer: integer => integer.Value.ToString());
        }
    }
}

[Funcky.DiscriminatedUnion]
public abstract partial record EmptyUnion
{
}

public static class EmptyUnionTest
{
    public static void EmptyUnionFn(EmptyUnion empty)
    {
        _ = empty.Match<string>();
        empty.Switch();
    }
}

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

[Funcky.DiscriminatedUnion(MatchResultTypeName = "TMatchResult")]
public abstract partial record UnionWithConflictingGenericType<TResult>
{
    public sealed partial record Variant : UnionWithConflictingGenericType<TResult>;
}
