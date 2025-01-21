﻿//HintName: DiscriminatedUnionGenerator.g.cs
// <auto-generated/>
#nullable enable

namespace Funcky.DiscriminatedUnion.Test.Sources
{
    partial record UnionWithPartitionUsage
    {
        [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "VERSION")]
        public abstract TResult Match<TResult>(global::System.Func<Success, TResult> success, global::System.Func<Warning, TResult> warning, global::System.Func<Error, TResult> error);
        
        [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "VERSION")]
        public abstract void Switch(global::System.Action<Success> success, global::System.Action<Warning> warning, global::System.Action<Error> error);
        
        partial record Success
        {
            [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "VERSION")]
            public override TResult Match<TResult>(global::System.Func<Success, TResult> success, global::System.Func<Warning, TResult> warning, global::System.Func<Error, TResult> error) => success(this);
            
            [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "VERSION")]
            public override void Switch(global::System.Action<Success> success, global::System.Action<Warning> warning, global::System.Action<Error> error) => success(this);
        }
        
        partial record Warning
        {
            [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "VERSION")]
            public override TResult Match<TResult>(global::System.Func<Success, TResult> success, global::System.Func<Warning, TResult> warning, global::System.Func<Error, TResult> error) => warning(this);
            
            [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "VERSION")]
            public override void Switch(global::System.Action<Success> success, global::System.Action<Warning> warning, global::System.Action<Error> error) => warning(this);
        }
        
        partial record Error
        {
            [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "VERSION")]
            public override TResult Match<TResult>(global::System.Func<Success, TResult> success, global::System.Func<Warning, TResult> warning, global::System.Func<Error, TResult> error) => error(this);
            
            [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "VERSION")]
            public override void Switch(global::System.Action<Success> success, global::System.Action<Warning> warning, global::System.Action<Error> error) => error(this);
        }
    }
    
    [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "VERSION")]
    public static partial class UnionWithPartitionUsageEnumerableExtensions
    {
        public static (global::System.Collections.Generic.IReadOnlyList<UnionWithPartitionUsage.Success> Success, global::System.Collections.Generic.IReadOnlyList<UnionWithPartitionUsage.Warning> Warning, global::System.Collections.Generic.IReadOnlyList<UnionWithPartitionUsage.Error> Error) Partition(this global::System.Collections.Generic.IEnumerable<UnionWithPartitionUsage> source)
        {
            var successItems = new global::System.Collections.Generic.List<UnionWithPartitionUsage.Success>();
            var warningItems = new global::System.Collections.Generic.List<UnionWithPartitionUsage.Warning>();
            var errorItems = new global::System.Collections.Generic.List<UnionWithPartitionUsage.Error>();
            foreach (var item in source)
            {
                item.Switch(success: successItems.Add, warning: warningItems.Add, error: errorItems.Add);
            }
            return (successItems.AsReadOnly(), warningItems.AsReadOnly(), errorItems.AsReadOnly());
        }
        
        public static TResult Partition<TResult>(this global::System.Collections.Generic.IEnumerable<UnionWithPartitionUsage> source, global::System.Func<global::System.Collections.Generic.IReadOnlyList<UnionWithPartitionUsage.Success>, global::System.Collections.Generic.IReadOnlyList<UnionWithPartitionUsage.Warning>, global::System.Collections.Generic.IReadOnlyList<UnionWithPartitionUsage.Error>, TResult> resultSelector)
        {
            var successItems = new global::System.Collections.Generic.List<UnionWithPartitionUsage.Success>();
            var warningItems = new global::System.Collections.Generic.List<UnionWithPartitionUsage.Warning>();
            var errorItems = new global::System.Collections.Generic.List<UnionWithPartitionUsage.Error>();
            foreach (var item in source)
            {
                item.Switch(success: successItems.Add, warning: warningItems.Add, error: errorItems.Add);
            }
            return resultSelector(successItems.AsReadOnly(), warningItems.AsReadOnly(), errorItems.AsReadOnly());
        }
    }
}