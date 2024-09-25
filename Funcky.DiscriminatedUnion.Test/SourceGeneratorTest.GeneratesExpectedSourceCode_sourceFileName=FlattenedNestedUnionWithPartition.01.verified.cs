﻿//HintName: DiscriminatedUnionGenerator.g.cs
// <auto-generated/>
#nullable enable

namespace Funcky.DiscriminatedUnion.Test
{
    partial record FlattenedNestedUnionWithPartition
    {
        [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "1.2.0.0")]
        public abstract TResult Match<TResult>(global::System.Func<Keyword, TResult> keyword, global::System.Func<Literal.Number.Integer, TResult> integer);
        
        [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "1.2.0.0")]
        public abstract void Switch(global::System.Action<Keyword> keyword, global::System.Action<Literal.Number.Integer> integer);
        
        partial record Keyword
        {
            [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "1.2.0.0")]
            public override TResult Match<TResult>(global::System.Func<Keyword, TResult> keyword, global::System.Func<Literal.Number.Integer, TResult> integer) => keyword(this);
            
            [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "1.2.0.0")]
            public override void Switch(global::System.Action<Keyword> keyword, global::System.Action<Literal.Number.Integer> integer) => keyword(this);
        }
        
        partial record Literal
        {
            partial record Number
            {
                partial record Integer
                {
                    [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "1.2.0.0")]
                    public override TResult Match<TResult>(global::System.Func<Keyword, TResult> keyword, global::System.Func<Literal.Number.Integer, TResult> integer) => integer(this);
                    
                    [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "1.2.0.0")]
                    public override void Switch(global::System.Action<Keyword> keyword, global::System.Action<Literal.Number.Integer> integer) => integer(this);
                }
            }
        }
    }
    
    [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "1.2.0.0")]
    public static partial class FlattenedNestedUnionWithPartitionEnumerableExtensions
    {
        public static (global::System.Collections.Generic.IReadOnlyList<FlattenedNestedUnionWithPartition.Keyword> keyword, global::System.Collections.Generic.IReadOnlyList<FlattenedNestedUnionWithPartition.Literal.Number.Integer> integer) Partition(this global::System.Collections.Generic.IEnumerable<FlattenedNestedUnionWithPartition> source)
        {
            var keywordItems = new global::System.Collections.Generic.List<FlattenedNestedUnionWithPartition.Keyword>();
            var integerItems = new global::System.Collections.Generic.List<FlattenedNestedUnionWithPartition.Literal.Number.Integer>();
            foreach (var item in source)
            {
                item.Switch(keyword: keywordItems.Add, integer: integerItems.Add);
            }
            return (keywordItems.AsReadOnly(), integerItems.AsReadOnly());
        }
        
        public static TResult Partition<TResult>(this global::System.Collections.Generic.IEnumerable<FlattenedNestedUnionWithPartition> source, global::System.Func<global::System.Collections.Generic.IReadOnlyList<FlattenedNestedUnionWithPartition.Keyword>, global::System.Collections.Generic.IReadOnlyList<FlattenedNestedUnionWithPartition.Literal.Number.Integer>, TResult> resultSelector)
        {
            var keywordItems = new global::System.Collections.Generic.List<FlattenedNestedUnionWithPartition.Keyword>();
            var integerItems = new global::System.Collections.Generic.List<FlattenedNestedUnionWithPartition.Literal.Number.Integer>();
            foreach (var item in source)
            {
                item.Switch(keyword: keywordItems.Add, integer: integerItems.Add);
            }
            return resultSelector(keywordItems.AsReadOnly(), integerItems.AsReadOnly());
        }
    }
}
