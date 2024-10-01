using System.Reflection;
using Funcky.DiscriminatedUnion.SourceGeneration;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Funcky.DiscriminatedUnion.Test;

[UsesVerify]
public sealed class SourceGeneratorTest
{
    [Theory]
    [InlineData("LogicallyNestedUnionWithFlatten")]
    [InlineData("LogicallyAndSyntacticallyNestedUnion")]
    [InlineData("LogicallyAndSyntacticallyNestedUnionWithFlatten")]
    [InlineData("UnionWithConflictingResultTypeName")]
    [InlineData("EmptyUnion")]
    [InlineData("KeywordsAsParameterNames")]
    [InlineData("UnionNestedInMultipleNamespaces")]
    [InlineData("GenericUnion")]
    [InlineData("DeeplyNestedUnion")]
    [InlineData("NonExhaustive")]
    [InlineData("JsonPolymorphic")]
    [InlineData("UnionWithPartitionUsage")]
    [InlineData("FlattenedUnionWithPartition")]
    [InlineData("FlattenedNestedUnionWithPartition")]
    public async Task GeneratesExpectedSourceCode(string sourceFileName) => await Verify(sourceFileName);

    [Fact]
    public void DoesNotEmitSourceFileWhenNoUnionsAreDefined()
    {
        var compilation = CreateCompilation();
        var driver = RunGenerator(compilation, out var outputCompilation);
        Assert.Empty(outputCompilation.GetDiagnostics());
        Assert.Single(driver.GetRunResult().GeneratedTrees);
    }

    private static async Task Verify(string sourceFileName)
    {
        var filePath = $"Sources/{sourceFileName}.cs";
        var compilation = CreateCompilation(CSharpSyntaxTree.ParseText(await File.ReadAllTextAsync(filePath)));
        var driver = RunGenerator(compilation, out var outputCompilation);
        Assert.Empty(outputCompilation.GetDiagnostics());
        await Verifier.Verify(driver).UseParameters(sourceFileName);
    }

    private static GeneratorDriver RunGenerator(CSharpCompilation compilation, out Compilation outputCompilation)
        => CSharpGeneratorDriver.Create(new DiscriminatedUnionGenerator())
            .RunGeneratorsAndUpdateCompilation(compilation, out outputCompilation, out _);

    private static CSharpCompilation CreateCompilation(params SyntaxTree[] syntaxTrees)
        => CSharpCompilation.Create(
            nameof(SourceGeneratorTest),
            syntaxTrees,
            new[]
            {
                MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location),
                MetadataReference.CreateFromFile(typeof(Attribute).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(System.Text.Json.JsonSerializer).Assembly.Location),
            },
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
}
