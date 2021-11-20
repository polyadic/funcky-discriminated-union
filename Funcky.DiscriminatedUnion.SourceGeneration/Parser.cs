using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Funcky.DiscriminatedUnion.SourceGeneration.SourceCodeSnippets;

namespace Funcky.DiscriminatedUnion.SourceGeneration
{
    internal static class Parser
    {
        public static bool IsSyntaxTargetForGeneration(SyntaxNode node)
            => node is ClassDeclarationSyntax { AttributeLists: { Count: > 0 } }
                    or RecordDeclarationSyntax { AttributeLists: { Count: > 0 } };

        public static TypeDeclarationSyntax? GetSemanticTargetForGeneration(GeneratorSyntaxContext context, CancellationToken cancellationToken)
        {
            var typeDeclaration = (TypeDeclarationSyntax)context.Node;
            return typeDeclaration.AttributeLists.Any(HasDiscriminatedUnionAttribute(context, cancellationToken))
                ? typeDeclaration
                : null;
        }

        public static DiscriminatedUnion? GetDiscriminatedUnionToEmit(TypeDeclarationSyntax typeDeclaration, Compilation compilation, CancellationToken cancellationToken)
        {
            var semanticModel = compilation.GetSemanticModel(typeDeclaration.SyntaxTree);
            var typeSymbol = semanticModel.GetDeclaredSymbol(typeDeclaration, cancellationToken);

            if (typeSymbol is null)
            {
                return null;
            }

            var (nonExhaustive, flatten, matchResultType) = ParseAttribute(typeSymbol);
            var isVariant = flatten ? IsVariantOfDiscriminatedUnionFlattened(typeSymbol, semanticModel) : IsVariantOfDiscriminatedUnion(typeSymbol, semanticModel);

            return new DiscriminatedUnion(
                Type: typeDeclaration,
                ParentTypes: typeDeclaration.Ancestors().OfType<TypeDeclarationSyntax>().ToList(),
                Namespace: FormatNamespace(typeSymbol),
                MatchResultTypeName: matchResultType ?? "TResult",
                MethodVisibility: nonExhaustive ? "internal" : "public",
                Variants: GetVariantTypeDeclarations(typeDeclaration, isVariant)
                    .Select(GetDiscriminatedUnionVariant(typeDeclaration, semanticModel))
                    .ToList());
        }

        private static AttributeData ParseAttribute(ITypeSymbol type)
        {
            var attribute = type.GetAttributes().Single(a => a.AttributeClass?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat.WithGlobalNamespaceStyle(SymbolDisplayGlobalNamespaceStyle.Omitted)) == AttributeFullName);
            var nonExhaustive = attribute.NamedArguments.Where(n => n.Key == AttributeProperties.NonExhaustive).Select(n => n.Value.Value).OfType<bool>().SingleOrDefault();
            var flatten = attribute.NamedArguments.Where(n => n.Key == AttributeProperties.Flatten).Select(n => n.Value.Value).OfType<bool>().SingleOrDefault();
            var matchResultType = attribute.NamedArguments.Where(n => n.Key == AttributeProperties.MatchResultTypeName).Select(n => n.Value.Value).OfType<string>().SingleOrDefault();
            return new AttributeData(nonExhaustive, flatten, matchResultType);
        }

        private static string? FormatNamespace(INamedTypeSymbol typeSymbol)
            => typeSymbol.ContainingNamespace?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat.WithGlobalNamespaceStyle(SymbolDisplayGlobalNamespaceStyle.Omitted));

        private static Func<TypeDeclarationSyntax, DiscriminatedUnionVariant> GetDiscriminatedUnionVariant(TypeDeclarationSyntax discrimatedUnionTypeDeclaration, SemanticModel semanticModel)
            => typeDeclaration => new DiscriminatedUnionVariant(
                typeDeclaration,
                ParentTypes: typeDeclaration.Ancestors().OfType<TypeDeclarationSyntax>().TakeWhile(t => t != discrimatedUnionTypeDeclaration).ToList(),
                ParameterName: FormatParameterName(typeDeclaration),
                LocalTypeName: semanticModel.GetDeclaredSymbol(typeDeclaration)!.ToMinimalDisplayString(semanticModel, NullableFlowState.NotNull, discrimatedUnionTypeDeclaration.SpanStart));

        private static IEnumerable<TypeDeclarationSyntax> GetVariantTypeDeclarations(TypeDeclarationSyntax discrimatedUnionTypeDeclaration, Func<TypeDeclarationSyntax, bool> isVariant)
        {
            var visitor = new VariantCollectingVisitor(isVariant);
            discrimatedUnionTypeDeclaration.Accept(visitor);
            return visitor.Variants;
        }

        private static Func<TypeDeclarationSyntax, bool> IsVariantOfDiscriminatedUnion(ITypeSymbol discriminatedUnionType, SemanticModel semanticModel)
            => node => semanticModel.GetDeclaredSymbol(node) is ITypeSymbol symbol
                && SymbolEqualityComparer.Default.Equals(symbol.BaseType, discriminatedUnionType);

        private static Func<TypeDeclarationSyntax, bool> IsVariantOfDiscriminatedUnionFlattened(ITypeSymbol discriminatedUnionType, SemanticModel semanticModel)
            => node => semanticModel.GetDeclaredSymbol(node) is ITypeSymbol symbol
                    && !symbol.IsAbstract
                    && GetBaseTypes(symbol).Any(t => SymbolEqualityComparer.Default.Equals(t, discriminatedUnionType));

        private static IEnumerable<ITypeSymbol> GetBaseTypes(ITypeSymbol symbol)
        {
            var currentSymbol = symbol.BaseType;
            while (currentSymbol is not null)
            {
                yield return currentSymbol;
                currentSymbol = currentSymbol.BaseType;
            }
        }

        private static string FormatParameterName(TypeDeclarationSyntax variant)
            => LowerCaseFirst(variant.Identifier.ToString());

        private static string LowerCaseFirst(string input) => char.ToLowerInvariant(input.First()) + input.Substring(1);

        private static Func<AttributeListSyntax, bool> HasDiscriminatedUnionAttribute(GeneratorSyntaxContext context, CancellationToken cancellationToken)
            => attributeList => attributeList.Attributes.Any(IsDiscriminatedUnionAttribute(context, cancellationToken));

        private static Func<AttributeSyntax, bool> IsDiscriminatedUnionAttribute(GeneratorSyntaxContext context, CancellationToken cancellationToken)
            => attribute
                => context.SemanticModel.GetSymbolInfo(attribute, cancellationToken).Symbol is IMethodSymbol attributeSymbol
                    && attributeSymbol.ContainingType.ToDisplayString() == AttributeFullName;

        private sealed record AttributeData(bool NonExhaustive, bool Flatten, string? MatchResultType);

        private sealed class VariantCollectingVisitor : CSharpSyntaxWalker
        {
            private readonly Func<TypeDeclarationSyntax, bool> _isVariant;

            public VariantCollectingVisitor(Func<TypeDeclarationSyntax, bool> isVariant) => _isVariant = isVariant;

            private readonly List<TypeDeclarationSyntax> _variants = new();

            public IReadOnlyList<TypeDeclarationSyntax> Variants => _variants;

            public override void VisitClassDeclaration(ClassDeclarationSyntax node)
            {
                if (_isVariant(node))
                {
                    _variants.Add(node);
                }

                base.VisitClassDeclaration(node);
            }

            public override void VisitRecordDeclaration(RecordDeclarationSyntax node)
            {
                if (_isVariant(node))
                {
                    _variants.Add(node);
                }

                base.VisitRecordDeclaration(node);
            }
        }
    }
}
