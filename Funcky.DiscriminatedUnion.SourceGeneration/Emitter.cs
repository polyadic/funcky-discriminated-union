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

                WriteGeneratedMethod(writer, $"{discriminatedUnion.MethodVisibility} abstract {FormatMatchMethodDeclaration(discriminatedUnion.ResultGenericTypeName, discriminatedUnion.Variants)};");
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
                WriteParentTypes(writer, variant.ParentTypes);

                writer.WriteLine(FormatPartialTypeDeclaration(variant.Type));
                writer.OpenScope();

                WriteGeneratedMethod(writer, $"{discriminatedUnion.MethodVisibility} override {FormatMatchMethodDeclaration(discriminatedUnion.ResultGenericTypeName, discriminatedUnion.Variants)} => {FormatVerbatimIdentifier(variant.ParameterName)}(this);");
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
            => $"TResult Match<{genericTypeName}>({string.Join(", ", variants.Select(variant => $"global::System.Func<{variant.LocalTypeName}, {genericTypeName}> {FormatVerbatimIdentifier(variant.ParameterName)}"))})";

        private static string FormatSwitchMethodDeclaration(IEnumerable<DiscriminatedUnionVariant> variants)
            => $"void Switch({string.Join(", ", variants.Select(variant => $"global::System.Action<{variant.LocalTypeName}> {FormatVerbatimIdentifier(variant.ParameterName)}"))})";

        private static string FormatPartialTypeDeclaration(TypeDeclarationSyntax typeDeclaration)
            => typeDeclaration is RecordDeclarationSyntax recordDeclaration && recordDeclaration.ClassOrStructKeyword.IsKind(SyntaxKind.None)
                ? $"partial {typeDeclaration.Keyword} {recordDeclaration.ClassOrStructKeyword} {typeDeclaration.Identifier}{typeDeclaration.TypeParameterList} {typeDeclaration.ConstraintClauses}"
                : $"partial {typeDeclaration.Keyword} {typeDeclaration.Identifier}{typeDeclaration.TypeParameterList} {typeDeclaration.ConstraintClauses}";

        private static string FormatVerbatimIdentifier(string identifier)
            => identifier.StartsWith("@")
                 ? identifier
                 : '@' + identifier;
    }

    internal static class IndentedTextWriterExtensions
    {
        public static IDisposable AutoCloseScopes(this IndentedTextWriter writer)
        {
            var indent = writer.Indent;
            return new LambdaDisposable(() =>
            {
                while (writer.Indent > indent)
                {
                    writer.CloseScope();
                }
            });
        }

        public static void OpenScope(this IndentedTextWriter writer)
        {
            writer.WriteLine("{");
            writer.Indent++;
        }

        private static void CloseScope(this IndentedTextWriter writer)
        {
            writer.Indent--;
            writer.WriteLine("}");
        }
    }

    internal sealed class LambdaDisposable : IDisposable
    {
        private readonly Action _disposeAction;

        public LambdaDisposable(Action disposeAction) => _disposeAction = disposeAction;

        public void Dispose() => _disposeAction();
    }
}
