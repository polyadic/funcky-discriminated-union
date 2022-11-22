﻿//HintName: DiscriminatedUnionGenerator.g.cs
// <auto-generated/>
#nullable enable

[global::System.Text.Json.Serialization.JsonDerivedType(typeof(global::Result.Ok), "Ok")]
[global::System.Text.Json.Serialization.JsonDerivedType(typeof(global::Result.Error), "Error")]
partial record Result
{
    [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "1.1.0.0")]
    public abstract TResult Match<TResult>(global::System.Func<Ok, TResult> ok, global::System.Func<Error, TResult> error);
    
    [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "1.1.0.0")]
    public abstract void Switch(global::System.Action<Ok> ok, global::System.Action<Error> error);
    
    partial record Ok
    {
        [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "1.1.0.0")]
        public override TResult Match<TResult>(global::System.Func<Ok, TResult> ok, global::System.Func<Error, TResult> error) => ok(this);
        
        [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "1.1.0.0")]
        public override void Switch(global::System.Action<Ok> ok, global::System.Action<Error> error) => ok(this);
    }
    
    partial record Error
    {
        [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "1.1.0.0")]
        public override TResult Match<TResult>(global::System.Func<Ok, TResult> ok, global::System.Func<Error, TResult> error) => error(this);
        
        [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "1.1.0.0")]
        public override void Switch(global::System.Action<Ok> ok, global::System.Action<Error> error) => error(this);
    }
}

partial class Nesting1<A, B, C>
{
    partial class Nesting2<D>
    {
        [global::System.Text.Json.Serialization.JsonDerivedType(typeof(global::Nesting1<, , >.Nesting2<>.Result<>.Ok), "Ok")]
        [global::System.Text.Json.Serialization.JsonDerivedType(typeof(global::Nesting1<, , >.Nesting2<>.Result<>.Error), "Error")]
        partial record Result<T>
        {
            [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "1.1.0.0")]
            public abstract TResult Match<TResult>(global::System.Func<Ok, TResult> ok, global::System.Func<Error, TResult> error);
            
            [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "1.1.0.0")]
            public abstract void Switch(global::System.Action<Ok> ok, global::System.Action<Error> error);
            
            partial record Ok
            {
                [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "1.1.0.0")]
                public override TResult Match<TResult>(global::System.Func<Ok, TResult> ok, global::System.Func<Error, TResult> error) => ok(this);
                
                [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "1.1.0.0")]
                public override void Switch(global::System.Action<Ok> ok, global::System.Action<Error> error) => ok(this);
            }
            
            partial record Error
            {
                [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "1.1.0.0")]
                public override TResult Match<TResult>(global::System.Func<Ok, TResult> ok, global::System.Func<Error, TResult> error) => error(this);
                
                [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "1.1.0.0")]
                public override void Switch(global::System.Action<Ok> ok, global::System.Action<Error> error) => error(this);
            }
        }
    }
}

[global::System.Text.Json.Serialization.JsonDerivedType(typeof(global::Shape.EquilateralTriangle), "EquilateralTriangle")]
partial record Shape
{
    [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "1.1.0.0")]
    public abstract TResult Match<TResult>(global::System.Func<Rectangle, TResult> rectangle, global::System.Func<Circle, TResult> circle, global::System.Func<EquilateralTriangle, TResult> equilateralTriangle);
    
    [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "1.1.0.0")]
    public abstract void Switch(global::System.Action<Rectangle> rectangle, global::System.Action<Circle> circle, global::System.Action<EquilateralTriangle> equilateralTriangle);
    
    partial record Rectangle
    {
        [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "1.1.0.0")]
        public override TResult Match<TResult>(global::System.Func<Rectangle, TResult> rectangle, global::System.Func<Circle, TResult> circle, global::System.Func<EquilateralTriangle, TResult> equilateralTriangle) => rectangle(this);
        
        [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "1.1.0.0")]
        public override void Switch(global::System.Action<Rectangle> rectangle, global::System.Action<Circle> circle, global::System.Action<EquilateralTriangle> equilateralTriangle) => rectangle(this);
    }
    
    partial record Circle
    {
        [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "1.1.0.0")]
        public override TResult Match<TResult>(global::System.Func<Rectangle, TResult> rectangle, global::System.Func<Circle, TResult> circle, global::System.Func<EquilateralTriangle, TResult> equilateralTriangle) => circle(this);
        
        [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "1.1.0.0")]
        public override void Switch(global::System.Action<Rectangle> rectangle, global::System.Action<Circle> circle, global::System.Action<EquilateralTriangle> equilateralTriangle) => circle(this);
    }
    
    partial record EquilateralTriangle
    {
        [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "1.1.0.0")]
        public override TResult Match<TResult>(global::System.Func<Rectangle, TResult> rectangle, global::System.Func<Circle, TResult> circle, global::System.Func<EquilateralTriangle, TResult> equilateralTriangle) => equilateralTriangle(this);
        
        [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "1.1.0.0")]
        public override void Switch(global::System.Action<Rectangle> rectangle, global::System.Action<Circle> circle, global::System.Action<EquilateralTriangle> equilateralTriangle) => equilateralTriangle(this);
    }
}
