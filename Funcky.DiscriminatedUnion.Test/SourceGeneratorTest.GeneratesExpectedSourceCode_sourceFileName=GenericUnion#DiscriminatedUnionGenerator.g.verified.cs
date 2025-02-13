﻿//HintName: DiscriminatedUnionGenerator.g.cs
// <auto-generated/>
#nullable enable

namespace Funcky.DiscriminatedUnion.Test
{
    partial record Result<T> where T : notnull
    {
        [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "VERSION")]
        internal abstract TResult Match<TResult>(global::System.Func<Ok, TResult> ok, global::System.Func<Error, TResult> error);
        
        [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "VERSION")]
        internal abstract void Switch(global::System.Action<Ok> ok, global::System.Action<Error> error);
        
        partial record Ok
        {
            [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "VERSION")]
            internal override TResult Match<TResult>(global::System.Func<Ok, TResult> ok, global::System.Func<Error, TResult> error) => ok(this);
            
            [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "VERSION")]
            internal override void Switch(global::System.Action<Ok> ok, global::System.Action<Error> error) => ok(this);
        }
        
        partial record Error
        {
            [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "VERSION")]
            internal override TResult Match<TResult>(global::System.Func<Ok, TResult> ok, global::System.Func<Error, TResult> error) => error(this);
            
            [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "VERSION")]
            internal override void Switch(global::System.Action<Ok> ok, global::System.Action<Error> error) => error(this);
        }
    }
}
