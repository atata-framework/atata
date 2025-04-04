namespace Atata;

public static class IEnumerableExtensions
{
    [Obsolete("Use either ToQuotedValuesListOfString() or ToDoubleQuotedValuesListOfString instead.")] // Obsolete since v4.0.0.
    public static string ToQuotedValuesListOfString(this IEnumerable<string> source, bool doubleQuotes) =>
        doubleQuotes
            ? ToDoubleQuotedValuesListOfString(source)
            : ToSingleQuotedValuesListOfString(source);

    public static string ToSingleQuotedValuesListOfString(this IEnumerable<string> source) =>
        $"'{string.Join("', '", source)}'";

    public static string ToDoubleQuotedValuesListOfString(this IEnumerable<string> source) =>
        $"\"{string.Join("\", \"", source)}\"";
}
