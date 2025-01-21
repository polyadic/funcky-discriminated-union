﻿//HintName: DiscriminatedUnionGenerator.g.cs
// <auto-generated/>
#nullable enable

namespace Funcky.DiscriminatedUnion.Test
{
    partial record SyntaxNodeFlattened
    {
        [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "VERSION")]
        public abstract TResult Match<TResult>(global::System.Func<Keyword, TResult> keyword, global::System.Func<Literal.Number.Integer, TResult> integer);
        
        [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "VERSION")]
        public abstract void Switch(global::System.Action<Keyword> keyword, global::System.Action<Literal.Number.Integer> integer);
        
        partial record Keyword
        {
            [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "VERSION")]
            public override TResult Match<TResult>(global::System.Func<Keyword, TResult> keyword, global::System.Func<Literal.Number.Integer, TResult> integer) => keyword(this);
            
            [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "VERSION")]
            public override void Switch(global::System.Action<Keyword> keyword, global::System.Action<Literal.Number.Integer> integer) => keyword(this);
        }
        
        partial record Literal
        {
            partial record Number
            {
                partial record Integer
                {
                    [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "VERSION")]
                    public override TResult Match<TResult>(global::System.Func<Keyword, TResult> keyword, global::System.Func<Literal.Number.Integer, TResult> integer) => integer(this);
                    
                    [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "VERSION")]
                    public override void Switch(global::System.Action<Keyword> keyword, global::System.Action<Literal.Number.Integer> integer) => integer(this);
                }
            }
        }
    }
}
