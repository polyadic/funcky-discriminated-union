using System.CodeDom.Compiler;

namespace Funcky.DiscriminatedUnion.SourceGeneration;

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

    private sealed class LambdaDisposable : IDisposable
    {
        private readonly Action _disposeAction;

        public LambdaDisposable(Action disposeAction) => _disposeAction = disposeAction;

        public void Dispose() => _disposeAction();
    }
}
