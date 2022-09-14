//HintName: DiscriminatedUnionGenerator.g.cs
// <auto-generated/>
#nullable enable

namespace Funcky.DiscriminatedUnion.Test
{
    partial record SyntaxNode
    {
        [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "1.0.0.0")]
        public abstract TResult Match<TResult>(global::System.Func<Keyword, TResult> keyword, global::System.Func<Literal, TResult> literal);
        
        [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "1.0.0.0")]
        public abstract void Switch(global::System.Action<Keyword> keyword, global::System.Action<Literal> literal);
        
        partial record Keyword
        {
            [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "1.0.0.0")]
            public override TResult Match<TResult>(global::System.Func<Keyword, TResult> keyword, global::System.Func<Literal, TResult> literal) => keyword(this);
            
            [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "1.0.0.0")]
            public override void Switch(global::System.Action<Keyword> keyword, global::System.Action<Literal> literal) => keyword(this);
        }
        
        partial record Literal
        {
            [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "1.0.0.0")]
            public override TResult Match<TResult>(global::System.Func<Keyword, TResult> keyword, global::System.Func<Literal, TResult> literal) => literal(this);
            
            [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "1.0.0.0")]
            public override void Switch(global::System.Action<Keyword> keyword, global::System.Action<Literal> literal) => literal(this);
        }
    }
}

namespace Funcky.DiscriminatedUnion.Test
{
    partial record SyntaxNode
    {
        partial record Literal
        {
            [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "1.0.0.0")]
            public abstract TResult Match<TResult>(global::System.Func<Integer, TResult> integer, global::System.Func<String, TResult> @string);
            
            [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "1.0.0.0")]
            public abstract void Switch(global::System.Action<Integer> integer, global::System.Action<String> @string);
            
            partial record Integer
            {
                [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "1.0.0.0")]
                public override TResult Match<TResult>(global::System.Func<Integer, TResult> integer, global::System.Func<String, TResult> @string) => integer(this);
                
                [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "1.0.0.0")]
                public override void Switch(global::System.Action<Integer> integer, global::System.Action<String> @string) => integer(this);
            }
            
            partial record String
            {
                [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "1.0.0.0")]
                public override TResult Match<TResult>(global::System.Func<Integer, TResult> integer, global::System.Func<String, TResult> @string) => @string(this);
                
                [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "1.0.0.0")]
                public override void Switch(global::System.Action<Integer> integer, global::System.Action<String> @string) => @string(this);
            }
        }
    }
}
