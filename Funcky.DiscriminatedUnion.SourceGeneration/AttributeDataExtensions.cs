using Microsoft.CodeAnalysis;

namespace Funcky.DiscriminatedUnion.SourceGeneration
{
    internal static class AttributeDataExtensions
    {
        public static TArgumentType? GetNamedArgumentOrDefault<TArgumentType>(this AttributeData attribute, string argumentName)
            => attribute.NamedArguments.Where(n => n.Key == argumentName).Select(n => n.Value.Value).OfType<TArgumentType>().SingleOrDefault();
    }
}
