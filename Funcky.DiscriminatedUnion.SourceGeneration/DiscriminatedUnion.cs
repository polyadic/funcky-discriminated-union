using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Funcky.DiscriminatedUnion.SourceGeneration;

internal sealed record DiscriminatedUnion(
    TypeDeclarationSyntax Type,
    IReadOnlyList<TypeDeclarationSyntax> ParentTypes,
    string? Namespace,
    string MethodVisibility,
    string MatchResultTypeName,
    IReadOnlyList<DiscriminatedUnionVariant> Variants,
    bool GeneratePartitionExtension);

internal sealed record DiscriminatedUnionVariant(
    TypeDeclarationSyntax Type,
    IReadOnlyList<TypeDeclarationSyntax> ParentTypes,
    string ParameterName,
    string LocalTypeName,
    string TypeOfTypeName,
    string JsonDerivedTypeDiscriminator,
    bool GenerateJsonDerivedTypeAttribute);
