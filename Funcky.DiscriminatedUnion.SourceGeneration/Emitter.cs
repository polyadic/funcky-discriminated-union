using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.CodeDom.Compiler;
using System.Text;
using static Funcky.DiscriminatedUnion.SourceGeneration.SourceCodeSnippets;

namespace Funcky.DiscriminatedUnion.SourceGeneration
{
    internal static class Emitter
    {
        public static string EmitDiscriminatedUnion(DiscriminatedUnion discriminatedUnion)
        {
            var stringBuilder = new StringBuilder();
            var writer = new IndentedTextWriter(new StringWriter(stringBuilder));

            using (writer.AutoCloseScopes())
            {
                WriteNamespace(writer, discriminatedUnion);

                WriteParentTypes(writer, discriminatedUnion.ParentTypes);

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

            return stringBuilder.ToString();
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

                WriteGeneratedMethod(writer, $"{discriminatedUnion.MethodVisibility} override {FormatMatchMethodDeclaration(discriminatedUnion.MatchResultTypeName, discriminatedUnion.Variants)} => {FormatVerbatimIdentifier(variant.ParameterName)}(this);");
                writer.WriteLine();
                WriteGeneratedMethod(writer, $"{discriminatedUnion.MethodVisibility} override {FormatSwitchMethodDeclaration(discriminatedUnion.Variants)} => {FormatVerbatimIdentifier(variant.ParameterName)}(this);");
            }
        }

        private static void WriteGeneratedMethod(IndentedTextWriter writer, string method)
        {
            writer.WriteLine(GeneratedCodeAttributeSource);
            writer.WriteLine(method);
        }

        private static string FormatMatchMethodDeclaration(string genericTypeName, IEnumerable<DiscriminatedUnionVariant> variants)
            => $"{genericTypeName} Match<{genericTypeName}>({string.Join(", ", variants.Select(variant => $"global::System.Func<{variant.LocalTypeName}, {genericTypeName}> {FormatVerbatimIdentifier(variant.ParameterName)}"))})";

        private static string FormatSwitchMethodDeclaration(IEnumerable<DiscriminatedUnionVariant> variants)
            => $"void Switch({string.Join(", ", variants.Select(variant => $"global::System.Action<{variant.LocalTypeName}> {FormatVerbatimIdentifier(variant.ParameterName)}"))})";

        private static string FormatPartialTypeDeclaration(TypeDeclarationSyntax typeDeclaration)
            => typeDeclaration is RecordDeclarationSyntax recordDeclaration
                ? CombineTokens("partial", typeDeclaration.Keyword, recordDeclaration.ClassOrStructKeyword, typeDeclaration.Identifier.ToString() + typeDeclaration.TypeParameterList, typeDeclaration.ConstraintClauses)
                : CombineTokens("partial", typeDeclaration.Keyword, typeDeclaration.Identifier.ToString() + typeDeclaration.TypeParameterList, typeDeclaration.ConstraintClauses);

        private static string CombineTokens(params object[] tokens)
            => string.Join(" ", tokens.Select(t => t.ToString()).Where(t => !string.IsNullOrEmpty(t)));

        private static string FormatVerbatimIdentifier(string identifier)
            => identifier.StartsWith("@")
                 ? identifier
                 : '@' + identifier;
    }    
}
