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
        writer.WriteLine(FormatPartialTypeDeclaration(discriminatedUnion.Type));
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
        writer.WriteLine($"{discriminatedUnion.MethodVisibility} static partial class {discriminatedUnion.Type.Identifier}EnumerableExtensions");
        writer.OpenScope();

        WriteTupleReturningPartitionExtension(discriminatedUnion, writer);
        writer.WriteLine();
        WritePartitionWithResultSelector(discriminatedUnion, writer);
    }

    private static void WriteTupleReturningPartitionExtension(DiscriminatedUnion discriminatedUnion, IndentedTextWriter writer)
    {
        using var methodScope = writer.AutoCloseScopes();

        writer.Write($"public static ");
        writer.Write("(");
        var partitionVariants = discriminatedUnion
            .Variants
            .Select(v => $"global::System.Collections.Generic.IReadOnlyList<{discriminatedUnion.Type.Identifier}.{v.LocalTypeName}> {v.ParameterName}");
        writer.Write(string.Join(", ", partitionVariants));
        writer.WriteLine(")");
        writer.WriteLine($" Partition(this global::System.Collections.Generic.IEnumerable<{discriminatedUnion.Type.Identifier}> source)");
        writer.OpenScope();

        WritePartitioningIntoLists(discriminatedUnion, writer);

        var items = discriminatedUnion.Variants.Select(v => $"{v.ParameterName}Items.AsReadOnly()");
        writer.WriteLine($"return new({string.Join(", ", items)});");
    }

    private static void WritePartitionWithResultSelector(DiscriminatedUnion discriminatedUnion, IndentedTextWriter writer)
    {
        using var methodScope = writer.AutoCloseScopes();

        writer.Write($"public static TResult Partition<{discriminatedUnion.MatchResultTypeName}>(this global::System.Collections.Generic.IEnumerable<{discriminatedUnion.Type.Identifier}> source, global::System.Func<");

        foreach (var variant in discriminatedUnion.Variants)
        {
            writer.Write("global::System.Collections.Generic.IReadOnlyList<");
            writer.Write(discriminatedUnion.Type.Identifier);
            writer.Write(".");
            writer.Write(variant.LocalTypeName);
            writer.WriteLine(">, ");
        }

        writer.WriteLine($"{discriminatedUnion.MatchResultTypeName}> resultSelector)");
        writer.OpenScope();

        WritePartitioningIntoLists(discriminatedUnion, writer);

        writer.Write("return resultSelector(");
        WriteCommaSeparated(
            discriminatedUnion.Variants,
            (v, w) =>
            {
                w.Write(v.ParameterName);
                w.Write("Items.AsReadOnly()");
            },
            writer);
        writer.WriteLine(");");
    }

    private static void WritePartitioningIntoLists(DiscriminatedUnion discriminatedUnion, IndentedTextWriter writer)
    {
        foreach (var variant in discriminatedUnion.Variants)
        {
            writer.WriteLine($"var {variant.ParameterName}Items = new global::System.Collections.Generic.List<{discriminatedUnion.Type.Identifier}.{variant.LocalTypeName}>();");
        }

        using (writer.AutoCloseScopes())
        {
            writer.WriteLine("foreach (var item in source)");
            writer.OpenScope();
            writer.Write("item.Switch(");
            WriteCommaSeparated(
                discriminatedUnion.Variants,
                (v, w) =>
                {
                    w.Write(v.ParameterName);
                    w.Write(": ");
                    w.Write(v.ParameterName);
                    w.Write("Items.Add");
                },
                writer);
            writer.WriteLine(");");
        }
    }

    private static void WriteNamespace(IndentedTextWriter writer, DiscriminatedUnion discriminatedUnion)
    {
        if (!string.IsNullOrEmpty(discriminatedUnion.Namespace))
        {
            writer.WriteLine($"namespace {discriminatedUnion.Namespace}");
            writer.OpenScope();
        }
    }

    private static void WriteParentTypes(IndentedTextWriter writer, IEnumerable<TypeDeclarationSyntax> parents)
    {
        foreach (var parent in parents.Reverse())
        {
            writer.WriteLine(FormatPartialTypeDeclaration(parent));
            writer.OpenScope();
        }
    }

    private static void WriteVariant(IndentedTextWriter writer, DiscriminatedUnion discriminatedUnion, DiscriminatedUnionVariant variant)
    {
        using (writer.AutoCloseScopes())
        {
            writer.WriteLine();

            WriteParentTypes(writer, variant.ParentTypes);

            writer.WriteLine(FormatPartialTypeDeclaration(variant.Type));
            writer.OpenScope();

            WriteGeneratedMethod(writer, $"{discriminatedUnion.MethodVisibility} override {FormatMatchMethodDeclaration(discriminatedUnion.MatchResultTypeName, discriminatedUnion.Variants)} => {FormatIdentifier(variant.ParameterName)}(this);");
            writer.WriteLine();
            WriteGeneratedMethod(writer, $"{discriminatedUnion.MethodVisibility} override {FormatSwitchMethodDeclaration(discriminatedUnion.Variants)} => {FormatIdentifier(variant.ParameterName)}(this);");
        }
    }

    private static void WriteGeneratedMethod(IndentedTextWriter writer, string method)
    {
        writer.WriteLine(GeneratedCodeAttributeSource);
        writer.WriteLine(method);
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
            writer.WriteLine($"[global::System.Text.Json.Serialization.JsonDerivedType(typeof({variant.TypeOfTypeName}), {SyntaxFactory.Literal(variant.JsonDerivedTypeDiscriminator)})]");
        }
    }

    private static string FormatMatchMethodDeclaration(string genericTypeName, IEnumerable<DiscriminatedUnionVariant> variants)
        => $"{genericTypeName} Match<{genericTypeName}>({string.Join(", ", variants.Select(variant => $"global::System.Func<{variant.LocalTypeName}, {genericTypeName}> {FormatIdentifier(variant.ParameterName)}"))})";

    private static string FormatSwitchMethodDeclaration(IEnumerable<DiscriminatedUnionVariant> variants)
        => $"void Switch({string.Join(", ", variants.Select(variant => $"global::System.Action<{variant.LocalTypeName}> {FormatIdentifier(variant.ParameterName)}"))})";

    private static string FormatPartialTypeDeclaration(TypeDeclarationSyntax typeDeclaration)
        => typeDeclaration is RecordDeclarationSyntax recordDeclaration
            ? CombineTokens("partial", typeDeclaration.Keyword, recordDeclaration.ClassOrStructKeyword, typeDeclaration.Identifier.ToString() + typeDeclaration.TypeParameterList, typeDeclaration.ConstraintClauses)
            : CombineTokens("partial", typeDeclaration.Keyword, typeDeclaration.Identifier.ToString() + typeDeclaration.TypeParameterList, typeDeclaration.ConstraintClauses);

    private static string CombineTokens(params object[] tokens)
        => string.Join(" ", tokens.Select(t => t.ToString()).Where(t => !string.IsNullOrEmpty(t)));

    private static string FormatIdentifier(string identifier)
        => IsIdentifier(identifier) ? '@' + identifier : identifier;

    private static bool IsIdentifier(string identifier)
        => SyntaxFacts.GetKeywordKind(identifier) != SyntaxKind.None;

    // Prevents extra string allocations compared to usages of string.Join before calling the writer.
    private static void WriteCommaSeparated<T>(IEnumerable<T> items, Action<T, IndentedTextWriter> action, IndentedTextWriter writer)
    {
        using var enumerator = items.GetEnumerator();

        if (enumerator.MoveNext())
        {
            action(enumerator.Current, writer);

            while (enumerator.MoveNext())
            {
                writer.Write(", ");
                action(enumerator.Current, writer);
            }
        }
    }
}
