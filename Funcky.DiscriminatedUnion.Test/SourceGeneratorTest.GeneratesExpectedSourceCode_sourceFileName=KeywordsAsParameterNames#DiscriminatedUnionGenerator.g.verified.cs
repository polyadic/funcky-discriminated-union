﻿//HintName: DiscriminatedUnionGenerator.g.cs
// <auto-generated/>
#nullable enable

namespace Funcky.DiscriminatedUnion.Test
{
    partial record Bool
    {
        [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "VERSION")]
        public abstract TResult Match<TResult>(global::System.Func<True, TResult> @true, global::System.Func<False, TResult> @false);
        
        [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "VERSION")]
        public abstract void Switch(global::System.Action<True> @true, global::System.Action<False> @false);
        
        partial record True
        {
            [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "VERSION")]
            public override TResult Match<TResult>(global::System.Func<True, TResult> @true, global::System.Func<False, TResult> @false) => @true(this);
            
            [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "VERSION")]
            public override void Switch(global::System.Action<True> @true, global::System.Action<False> @false) => @true(this);
        }
        
        partial record False
        {
            [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "VERSION")]
            public override TResult Match<TResult>(global::System.Func<True, TResult> @true, global::System.Func<False, TResult> @false) => @false(this);
            
            [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "VERSION")]
            public override void Switch(global::System.Action<True> @true, global::System.Action<False> @false) => @false(this);
        }
    }
}
