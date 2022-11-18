using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Funcky.DiscriminatedUnion.SourceGeneration;

internal sealed record DiscriminatedUnion(
    TypeDeclarationSyntax Type,
    IReadOnlyList<TypeDeclarationSyntax> ParentTypes,
    string? Namespace,
    string MethodVisibility,
    string MatchResultTypeName,
    bool GenerateJsonDerivedTypeAttributes, // TODO: include a list of already present JsonDerivedTypes
    IReadOnlyList<DiscriminatedUnionVariant> Variants);

internal sealed record DiscriminatedUnionVariant(
    TypeDeclarationSyntax Type,
    IReadOnlyList<TypeDeclarationSyntax> ParentTypes,
    string ParameterName,
    string LocalTypeName,
    string JsonDerivedTypeDiscriminator);
