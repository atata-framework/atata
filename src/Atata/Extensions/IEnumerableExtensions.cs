namespace Atata;

public static class IEnumerableExtensions
{
    public static string ToSingleQuotedValuesListOfString(this IEnumerable<string> source) =>
        $"'{string.Join("', '", source)}'";

    public static string ToDoubleQuotedValuesListOfString(this IEnumerable<string> source) =>
        $"\"{string.Join("\", \"", source)}\"";
}
