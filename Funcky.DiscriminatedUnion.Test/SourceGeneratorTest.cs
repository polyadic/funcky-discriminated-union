using System.Reflection;
using Funcky.DiscriminatedUnion.SourceGeneration;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Funcky.DiscriminatedUnion.Test;

[UsesVerify]
public class SourceGeneratorTest
{
    [Fact]
    public async Task GeneratesCorrectly() => await Verify("Sources/IntegrationTest.cs");

    private static async Task Verify(string testName)
    {
        var compilation = CreateCompilation(CSharpSyntaxTree.ParseText(await File.ReadAllTextAsync(testName)));
        var driver = RunGenerator(compilation, out var outputCompilation);
        Assert.Empty(outputCompilation.GetDiagnostics());
        await Verifier.Verify(driver);
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
            },
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
}
