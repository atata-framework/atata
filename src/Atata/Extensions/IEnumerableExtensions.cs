#nullable enable

namespace Atata;

public static class IEnumerableExtensions
{
    public static string ToQuotedValuesListOfString(this IEnumerable<string> source, bool doubleQuotes = false)
    {
        char quotesCharacter = doubleQuotes ? '"' : '\'';
        string separator = "{0}, {0}".FormatWith(quotesCharacter);
        return "{0}{1}{0}".FormatWith(quotesCharacter, string.Join(separator, source));
    }
}
