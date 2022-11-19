using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using static Funcky.DiscriminatedUnion.SourceGeneration.Functional;

namespace Funcky.DiscriminatedUnion.SourceGeneration;

internal static partial class Parser
{
    private const string JsonPolymorphicAttributeName = "System.Text.Json.Serialization.JsonPolymorphicAttribute";
    private const string JsonDerivedTypeAttributeName = "System.Text.Json.Serialization.JsonDerivedTypeAttribute";

    private static Func<INamedTypeSymbol, bool> GenerateJsonDerivedTypeAttribute(INamedTypeSymbol discriminatedUnion)
    {
        var generateJsonDerivedTypeAttributes = Lazy(() => discriminatedUnion.GetAttributes().Any(IsJsonPolymorphicAttribute));
        var jsonDerivedTypes = Lazy(() => GetJsonDerivedTypes(discriminatedUnion));
        return variant => generateJsonDerivedTypeAttributes.Value && !jsonDerivedTypes.Value.Contains(variant);
    }

    private static ImmutableHashSet<INamedTypeSymbol> GetJsonDerivedTypes(INamedTypeSymbol discriminatedUnion)
        => discriminatedUnion.GetAttributes()
            .Select(GetJsonDerivedType)
            .Where(t => t is not null)!
            .ToImmutableHashSet<INamedTypeSymbol>(SymbolEqualityComparer.Default);

    private static bool IsJsonPolymorphicAttribute(AttributeData attribute)
        => attribute.AttributeClass?.ToDisplayString() is JsonPolymorphicAttributeName or JsonDerivedTypeAttributeName;

    private static INamedTypeSymbol? GetJsonDerivedType(AttributeData attribute)
        => attribute.AttributeClass?.ToDisplayString() is JsonDerivedTypeAttributeName
            && attribute.ConstructorArguments.First() is { Kind: TypedConstantKind.Type, Value: INamedTypeSymbol value }
                ? value
                : null;
}
