using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Funcky.DiscriminatedUnion.SourceGeneration.Emitter;
using static Funcky.DiscriminatedUnion.SourceGeneration.Parser;
using static Funcky.DiscriminatedUnion.SourceGeneration.SourceCodeSnippets;

namespace Funcky.DiscriminatedUnion.SourceGeneration;

[Generator(LanguageNames.CSharp)]
public sealed class DiscriminatedUnionGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(context => context.AddSource("DiscriminatedUnionAttribute.g.cs", DiscriminatedUnionAttributeSource));

        var code = context.SyntaxProvider.CreateSyntaxProvider(static (node, _) => IsSyntaxTargetForGeneration(node), GetSemanticTargetForGeneration)
            .WhereNotNull()
            .Combine(context.CompilationProvider)
            // Use a custom comparer that ignores the compilation. We want to avoid regenerating for discriminated unions
            // that haven't been changed, but any change to a discriminated union will change the Compilation, so we ignore
            // the Compilation for purposes of caching.
            .WithComparer(new SelectorComparer<(TypeDeclarationSyntax Left, Compilation Right), TypeDeclarationSyntax>(state => state.Left))
            .Select((state, cancellationToken) => Emit(state.Left, state.Right, cancellationToken))
            .WhereNotNull()
            .Collect();

        context.RegisterSourceOutput(code, (context, code) => context.AddSource("DiscriminatedUnionGenerator.g.cs", GeneratedFileHeadersSource + string.Join(Environment.NewLine, code)));
    }

    private static string? Emit(TypeDeclarationSyntax typeDeclaration, Compilation compilation, CancellationToken cancellationToken)
        => GetDiscriminatedUnionToEmit(typeDeclaration, compilation, cancellationToken) is { } discrimatedUnion
            ? EmitDiscriminatedUnion(discrimatedUnion)
            : null;
}
