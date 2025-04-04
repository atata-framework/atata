#nullable enable

namespace Atata;

public static class TermMatchExtensions
{
    public static string CreateXPathCondition(this TermMatch match, string value, string operand = ".")
    {
        if (match != TermMatch.Equals)
            value.CheckNotNullOrEmpty(nameof(value));

        operand.CheckNotNullOrEmpty(nameof(operand));

        string valueString = XPathString.ConvertTo(value);

        return match switch
        {
            TermMatch.Contains => $"contains({operand}, {valueString})",
            TermMatch.Equals => value is null && operand != "."
                ? $"not({operand})"
                : $"normalize-space({operand}) = {valueString}",
            TermMatch.StartsWith => $"starts-with(normalize-space({operand}), {valueString})",
            TermMatch.EndsWith => $"substring(normalize-space({operand}), string-length(normalize-space({operand})) - {value.Length - 1}) = {valueString}",
            _ => throw ExceptionFactory.CreateForUnsupportedEnumValue(match, nameof(match)),
        };
    }

    public static string CreateXPathCondition(this TermMatch match, string[] values, string operand = ".")
    {
        values.CheckNotNull(nameof(values));
        operand.CheckNotNull(nameof(operand));

        return string.Join(" or ", values.Select(x => match.CreateXPathCondition(x, operand)));
    }

    public static bool IsMatch(this TermMatch match, string text, params string[] terms)
    {
        var predicate = match.GetPredicate();
        return terms.Any(term => predicate(text, term));
    }

    public static Func<string, string, bool> GetPredicate(this TermMatch match) =>
        match.GetPredicate(StringComparison.Ordinal);

    public static Func<string, string, bool> GetPredicate(this TermMatch match, StringComparison stringComparison) =>
        match switch
        {
            TermMatch.Contains => (text, term) => text != null && text.IndexOf(term, stringComparison) != -1,
            TermMatch.Equals => (text, term) => text != null && text.Equals(term, stringComparison),
            TermMatch.StartsWith => (text, term) => text != null && text.StartsWith(term, stringComparison),
            TermMatch.EndsWith => (text, term) => text != null && text.EndsWith(term, stringComparison),
            _ => throw ExceptionFactory.CreateForUnsupportedEnumValue(match, nameof(match))
        };

    internal static string GetShouldText(this TermMatch match) =>
        match switch
        {
            TermMatch.Contains => "contain",
            TermMatch.Equals => "equal",
            TermMatch.StartsWith => "start with",
            TermMatch.EndsWith => "end with",
            _ => throw ExceptionFactory.CreateForUnsupportedEnumValue(match, nameof(match))
        };

    internal static string FormatComponentName(this TermMatch match, string[] values)
    {
        var format = match switch
        {
            TermMatch.Contains => "Containing '{0}'",
            TermMatch.Equals => "{0}",
            TermMatch.StartsWith => "Starting with '{0}'",
            TermMatch.EndsWith => "Ending with '{0}'",
            _ => throw ExceptionFactory.CreateForUnsupportedEnumValue(match, nameof(match))
        };
        string combinedValues = TermResolver.ToDisplayString(values);

        return string.Format(format, combinedValues);
    }
}
