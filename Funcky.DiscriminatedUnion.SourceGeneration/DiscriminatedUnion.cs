using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Funcky.DiscriminatedUnion.SourceGeneration
{
    internal sealed record DiscriminatedUnion(
        TypeDeclarationSyntax Type,
        IReadOnlyList<TypeDeclarationSyntax> ParentTypes,
        string? Namespace,
        string MethodVisibility,
        string MatchResultType,
        IReadOnlyList<DiscriminatedUnionVariant> Variants);

    internal sealed record DiscriminatedUnionVariant(
        TypeDeclarationSyntax Type,
        IReadOnlyList<TypeDeclarationSyntax> ParentTypes,
        string ParameterName,
        string LocalTypeName);
}
