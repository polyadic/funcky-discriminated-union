﻿//HintName: DiscriminatedUnionGenerator.g.cs
// <auto-generated/>
#nullable enable

namespace Funcky.DiscriminatedUnion.Test
{
    partial record FlattenedUnionWithPartition
    {
        [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "VERSION")]
        public abstract TResult Match<TResult>(global::System.Func<Keyword, TResult> keyword, global::System.Func<Integer, TResult> integer);
        
        [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "VERSION")]
        public abstract void Switch(global::System.Action<Keyword> keyword, global::System.Action<Integer> integer);
        
        partial record Keyword
        {
            [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "VERSION")]
            public override TResult Match<TResult>(global::System.Func<Keyword, TResult> keyword, global::System.Func<Integer, TResult> integer) => keyword(this);
            
            [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "VERSION")]
            public override void Switch(global::System.Action<Keyword> keyword, global::System.Action<Integer> integer) => keyword(this);
        }
        
        partial record Integer
        {
            [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "VERSION")]
            public override TResult Match<TResult>(global::System.Func<Keyword, TResult> keyword, global::System.Func<Integer, TResult> integer) => integer(this);
            
            [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "VERSION")]
            public override void Switch(global::System.Action<Keyword> keyword, global::System.Action<Integer> integer) => integer(this);
        }
    }
    
    [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "VERSION")]
    public static partial class FlattenedUnionWithPartitionEnumerableExtensions
    {
        public static (global::System.Collections.Generic.IReadOnlyList<FlattenedUnionWithPartition.Keyword> Keyword, global::System.Collections.Generic.IReadOnlyList<FlattenedUnionWithPartition.Integer> Integer) Partition(this global::System.Collections.Generic.IEnumerable<FlattenedUnionWithPartition> source)
        {
            var keywordItems = new global::System.Collections.Generic.List<FlattenedUnionWithPartition.Keyword>();
            var integerItems = new global::System.Collections.Generic.List<FlattenedUnionWithPartition.Integer>();
            foreach (var item in source)
            {
                item.Switch(keyword: keywordItems.Add, integer: integerItems.Add);
            }
            return (keywordItems.AsReadOnly(), integerItems.AsReadOnly());
        }
        
        public static TResult Partition<TResult>(this global::System.Collections.Generic.IEnumerable<FlattenedUnionWithPartition> source, global::System.Func<global::System.Collections.Generic.IReadOnlyList<FlattenedUnionWithPartition.Keyword>, global::System.Collections.Generic.IReadOnlyList<FlattenedUnionWithPartition.Integer>, TResult> resultSelector)
        {
            var keywordItems = new global::System.Collections.Generic.List<FlattenedUnionWithPartition.Keyword>();
            var integerItems = new global::System.Collections.Generic.List<FlattenedUnionWithPartition.Integer>();
            foreach (var item in source)
            {
                item.Switch(keyword: keywordItems.Add, integer: integerItems.Add);
            }
            return resultSelector(keywordItems.AsReadOnly(), integerItems.AsReadOnly());
        }
    }
}
