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

    public static void WriteInterpolated(this IndentedTextWriter writer, CollectingInterpolatedStringHandler value)
    {
        foreach (var item in value.GetItems())
        {
            writer.Write(item?.ToString());
        }
    }

    public static void WriteLineInterpolated(this IndentedTextWriter writer, CollectingInterpolatedStringHandler value)
    {
        writer.WriteInterpolated(value);
        writer.WriteLine();
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
