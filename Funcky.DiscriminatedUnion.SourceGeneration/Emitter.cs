using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.CodeDom.Compiler;
using System.Text;
using static Funcky.DiscriminatedUnion.SourceGeneration.SourceCodeSnippets;

namespace Funcky.DiscriminatedUnion.SourceGeneration;

internal static class Emitter
{
    public static string EmitDiscriminatedUnion(DiscriminatedUnion discriminatedUnion)
    {
        var stringBuilder = new StringBuilder();
        var writer = new IndentedTextWriter(new StringWriter(stringBuilder));

        using (writer.AutoCloseScopes())
        {
            WriteNamespace(writer, discriminatedUnion);

            WriteUnionType(discriminatedUnion, writer);

            if (discriminatedUnion.GeneratePartitionExtension)
            {
                writer.WriteLine();
                WritePartitionExtensions(discriminatedUnion, writer);
            }
        }

        return stringBuilder.ToString();
    }

    private static void WriteUnionType(DiscriminatedUnion discriminatedUnion, IndentedTextWriter writer)
    {
        using var scope = writer.AutoCloseScopes();

        WriteParentTypes(writer, discriminatedUnion.ParentTypes);

        WriteJsonDerivedTypeAttributes(writer, discriminatedUnion);
        writer.WriteLineInterpolated(FormatPartialTypeDeclaration(discriminatedUnion.Type));
        writer.OpenScope();

        WriteGeneratedMethod(writer, $"{discriminatedUnion.MethodVisibility} abstract {FormatMatchMethodDeclaration(discriminatedUnion.MatchResultTypeName, discriminatedUnion.Variants)};");
        writer.WriteLine();
        WriteGeneratedMethod(writer, $"{discriminatedUnion.MethodVisibility} abstract {FormatSwitchMethodDeclaration(discriminatedUnion.Variants)};");

        foreach (var variant in discriminatedUnion.Variants)
        {
            WriteVariant(writer, discriminatedUnion, variant);
        }
    }

    private static void WritePartitionExtensions(DiscriminatedUnion discriminatedUnion, IndentedTextWriter writer)
    {
        using var scope = writer.AutoCloseScopes();

        writer.WriteLine(GeneratedCodeAttributeSource);
        writer.WriteLineInterpolated($"{discriminatedUnion.MethodVisibility} static partial class {discriminatedUnion.Type.Identifier}EnumerableExtensions");
        writer.OpenScope();

        WriteTupleReturningPartitionExtension(discriminatedUnion, writer);
        writer.WriteLine();
        WritePartitionWithResultSelector(discriminatedUnion, writer);
    }

    private static void WriteTupleReturningPartitionExtension(DiscriminatedUnion discriminatedUnion, IndentedTextWriter writer)
    {
        using var methodScope = writer.AutoCloseScopes();

        var namedResultPartitions = discriminatedUnion
            .Variants
            .JoinToInterpolation(
                v => $"global::System.Collections.Generic.IReadOnlyList<{discriminatedUnion.Type.Identifier}.{v.LocalTypeName}> {v.ParameterName}",
                ", ");

        writer.WriteLineInterpolated($"public static ({namedResultPartitions}) Partition(this global::System.Collections.Generic.IEnumerable<{discriminatedUnion.Type.Identifier}> source)");
        writer.OpenScope();

        WritePartitioningIntoLists(discriminatedUnion, writer);

        writer.WriteLineInterpolated($"return ({discriminatedUnion.Variants.JoinToInterpolation(v => $"{v.ParameterName}Items.AsReadOnly()", ", ")});");
    }

    private static void WritePartitionWithResultSelector(DiscriminatedUnion discriminatedUnion, IndentedTextWriter writer)
    {
        using var methodScope = writer.AutoCloseScopes();

        writer.WriteInterpolated($"public static TResult Partition<{discriminatedUnion.MatchResultTypeName}>(this global::System.Collections.Generic.IEnumerable<{discriminatedUnion.Type.Identifier}> source, global::System.Func<");

        foreach (var variant in discriminatedUnion.Variants)
        {
            writer.WriteInterpolated($"global::System.Collections.Generic.IReadOnlyList<{discriminatedUnion.Type.Identifier}.{variant.LocalTypeName}>, ");
        }

        writer.WriteLineInterpolated($"{discriminatedUnion.MatchResultTypeName}> resultSelector)");
        writer.OpenScope();

        WritePartitioningIntoLists(discriminatedUnion, writer);

        writer.WriteLineInterpolated($"return resultSelector({discriminatedUnion.Variants.JoinToInterpolation(v => $"{v.ParameterName}Items.AsReadOnly()", ", ")});");
    }

    private static void WritePartitioningIntoLists(DiscriminatedUnion discriminatedUnion, IndentedTextWriter writer)
    {
        foreach (var variant in discriminatedUnion.Variants)
        {
            writer.WriteLineInterpolated($"var {variant.ParameterName}Items = new global::System.Collections.Generic.List<{discriminatedUnion.Type.Identifier}.{variant.LocalTypeName}>();");
        }

        using (writer.AutoCloseScopes())
        {
            writer.WriteLine("foreach (var item in source)");
            writer.OpenScope();
            writer.WriteLineInterpolated($"item.Switch({discriminatedUnion.Variants.JoinToInterpolation(v => $"{v.ParameterName}: {v.ParameterName}Items.Add", ", ")});");
        }
    }

    private static void WriteNamespace(IndentedTextWriter writer, DiscriminatedUnion discriminatedUnion)
    {
        if (!string.IsNullOrEmpty(discriminatedUnion.Namespace))
        {
            writer.WriteLineInterpolated($"namespace {discriminatedUnion.Namespace}");
            writer.OpenScope();
        }
    }

    private static void WriteParentTypes(IndentedTextWriter writer, IEnumerable<TypeDeclarationSyntax> parents)
    {
        foreach (var parent in parents.Reverse())
        {
            writer.WriteLineInterpolated(FormatPartialTypeDeclaration(parent));
            writer.OpenScope();
        }
    }

    private static void WriteVariant(IndentedTextWriter writer, DiscriminatedUnion discriminatedUnion, DiscriminatedUnionVariant variant)
    {
        using (writer.AutoCloseScopes())
        {
            writer.WriteLine();

            WriteParentTypes(writer, variant.ParentTypes);

            writer.WriteLineInterpolated(FormatPartialTypeDeclaration(variant.Type));
            writer.OpenScope();

            WriteGeneratedMethod(writer, $"{discriminatedUnion.MethodVisibility} override {FormatMatchMethodDeclaration(discriminatedUnion.MatchResultTypeName, discriminatedUnion.Variants)} => {FormatIdentifier(variant.ParameterName)}(this);");
            writer.WriteLine();
            WriteGeneratedMethod(writer, $"{discriminatedUnion.MethodVisibility} override {FormatSwitchMethodDeclaration(discriminatedUnion.Variants)} => {FormatIdentifier(variant.ParameterName)}(this);");
        }
    }

    private static void WriteGeneratedMethod(IndentedTextWriter writer, CollectingInterpolatedStringHandler method)
    {
        writer.WriteLine(GeneratedCodeAttributeSource);
        writer.WriteLineInterpolated(method);
    }

    private static void WriteJsonDerivedTypeAttributes(IndentedTextWriter writer, DiscriminatedUnion discriminatedUnion)
    {
        foreach (var variant in discriminatedUnion.Variants)
        {
            WriteJsonDerivedTypeAttribute(writer, variant);
        }
    }

    private static void WriteJsonDerivedTypeAttribute(IndentedTextWriter writer, DiscriminatedUnionVariant variant)
    {
        if (variant.GenerateJsonDerivedTypeAttribute)
        {
            writer.WriteLineInterpolated($"[global::System.Text.Json.Serialization.JsonDerivedType(typeof({variant.TypeOfTypeName}), {SyntaxFactory.Literal(variant.JsonDerivedTypeDiscriminator)})]");
        }
    }

    private static CollectingInterpolatedStringHandler FormatMatchMethodDeclaration(string genericTypeName, IEnumerable<DiscriminatedUnionVariant> variants)
        => $"{genericTypeName} Match<{genericTypeName}>({variants.JoinToInterpolation(v => $"global::System.Func<{v.LocalTypeName}, {genericTypeName}> {FormatIdentifier(v.ParameterName)}", ", ")})";

    private static CollectingInterpolatedStringHandler FormatSwitchMethodDeclaration(IEnumerable<DiscriminatedUnionVariant> variants)
        => $"void Switch({variants.JoinToInterpolation(v => $"global::System.Action<{v.LocalTypeName}> {FormatIdentifier(v.ParameterName)}", ", ")})";

    private static CollectingInterpolatedStringHandler FormatPartialTypeDeclaration(TypeDeclarationSyntax typeDeclaration)
        => typeDeclaration is RecordDeclarationSyntax recordDeclaration
            ? CombineTokens("partial", typeDeclaration.Keyword, recordDeclaration.ClassOrStructKeyword, typeDeclaration.Identifier.ToString() + typeDeclaration.TypeParameterList, typeDeclaration.ConstraintClauses)
            : CombineTokens("partial", typeDeclaration.Keyword, typeDeclaration.Identifier.ToString() + typeDeclaration.TypeParameterList, typeDeclaration.ConstraintClauses);

    private static CollectingInterpolatedStringHandler CombineTokens(params object[] tokens)
        => tokens.Select(t => t.ToString()).Where(t => !string.IsNullOrEmpty(t)).JoinToInterpolation(" ");

    private static CollectingInterpolatedStringHandler FormatIdentifier(string identifier)
        => $"{(IsIdentifier(identifier) ? "@" : string.Empty)}{identifier}";

    private static bool IsIdentifier(string identifier)
        => SyntaxFacts.GetKeywordKind(identifier) != SyntaxKind.None;
}
