//HintName: DiscriminatedUnionGenerator.g.cs
// <auto-generated/>
#nullable enable

partial record UnionWithConflictingGenericType<TResult>
{
    [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "1.0.0.0")]
    public abstract TMatchResult Match<TMatchResult>(global::System.Func<Variant, TMatchResult> variant);
    
    [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "1.0.0.0")]
    public abstract void Switch(global::System.Action<Variant> variant);
    
    partial record Variant
    {
        [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "1.0.0.0")]
        public override TMatchResult Match<TMatchResult>(global::System.Func<Variant, TMatchResult> variant) => variant(this);
        
        [global::System.CodeDom.Compiler.GeneratedCode("Funcky.DiscriminatedUnion.SourceGeneration", "1.0.0.0")]
        public override void Switch(global::System.Action<Variant> variant) => variant(this);
    }
}
