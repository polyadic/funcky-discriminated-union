using System.Reflection;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Funcky.DiscriminatedUnion.SourceGeneration;

internal static class SourceCodeSnippets
{
    public const string AttributeFullName = "Funcky.DiscriminatedUnionAttribute";

    // language=c#
    public const string GeneratedFileHeadersSource =
        "// <auto-generated/>\n" +
        "#nullable enable";

    // language=c#
    public const string DiscriminatedUnionAttributeSource =
        $$"""
        {{GeneratedFileHeadersSource}}

        namespace Funcky
        {
            [global::System.Diagnostics.Conditional("Funcky_DiscriminatedUnion")]
            [global::System.AttributeUsage(global::System.AttributeTargets.Class)]
            internal sealed class DiscriminatedUnionAttribute : global::System.Attribute
            {
                /// <summary>Allow only consumers in the same assembly to use the exhaustive <c>Match</c> and <c>Switch</c> methods.</summary>
                public bool {{AttributeProperties.NonExhaustive}} { get; set; }

                /// <summary>Generates exhaustive <c>Match</c> and <c>Switch</c> methods for the entire type hierarchy.</summary>
                public bool {{AttributeProperties.Flatten}} { get; set; }

                /// <summary>Customized the generic type name used for the result in the generated <c>Match</c> methods. Defaults to <c>TResult</c>.</summary>
                public string? {{AttributeProperties.MatchResultTypeName}} { get; set; }
            }
        }

        """;

    private static readonly AssemblyName GeneratorAssemblyName = typeof(DiscriminatedUnionGenerator).Assembly.GetName();

    public static readonly string GeneratedCodeAttributeSource = $"[global::System.CodeDom.Compiler.GeneratedCode(" +
        $"{Literal(GeneratorAssemblyName.Name)}, " +
        $"{Literal(GeneratorAssemblyName.Version.ToString())})]";

    public static class AttributeProperties
    {
        public const string NonExhaustive = "NonExhaustive";
        public const string Flatten = "Flatten";
        public const string MatchResultTypeName = "MatchResultTypeName";
    }
}
