using System;
using System.Linq;

namespace Atata
{
    public static class TermMatchExtensions
    {
        public static string CreateXPathCondition(this TermMatch match, string value, string operand = ".")
        {
            if (match != TermMatch.Equals)
                value.CheckNotNullOrEmpty(nameof(value));

            operand.CheckNotNullOrEmpty(nameof(operand));

            string valueString = XPathString.ConvertTo(value);

            switch (match)
            {
                case TermMatch.Contains:
                    return $"contains({operand}, {valueString})";
                case TermMatch.Equals:
                    return value is null && operand != "."
                        ? $"not({operand})"
                        : $"normalize-space({operand}) = {valueString}";
                case TermMatch.StartsWith:
                    return $"starts-with(normalize-space({operand}), {valueString})";
                case TermMatch.EndsWith:
                    return $"substring(normalize-space({operand}), string-length(normalize-space({operand})) - {value.Length - 1}) = {valueString}";
                default:
                    throw ExceptionFactory.CreateForUnsupportedEnumValue(match, nameof(match));
            }
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

        public static Func<string, string, bool> GetPredicate(this TermMatch match)
        {
            switch (match)
            {
                case TermMatch.Contains:
                    return (text, term) => text.Contains(term);
                case TermMatch.Equals:
                    return (text, term) => text.Trim() == term;
                case TermMatch.StartsWith:
                    return (text, term) => text.Trim().StartsWith(term, StringComparison.Ordinal);
                case TermMatch.EndsWith:
                    return (text, term) => text.Trim().EndsWith(term, StringComparison.Ordinal);
                default:
                    throw ExceptionFactory.CreateForUnsupportedEnumValue(match, nameof(match));
            }
        }

        internal static string GetShouldText(this TermMatch match)
        {
            switch (match)
            {
                case TermMatch.Contains:
                    return "contain";
                case TermMatch.Equals:
                    return "equal";
                case TermMatch.StartsWith:
                    return "start with";
                case TermMatch.EndsWith:
                    return "end with";
                default:
                    throw ExceptionFactory.CreateForUnsupportedEnumValue(match, nameof(match));
            }
        }

        internal static string FormatComponentName(this TermMatch match, string[] values)
        {
            string format;

            switch (match)
            {
                case TermMatch.Contains:
                    format = "Containing '{0}'";
                    break;
                case TermMatch.Equals:
                    format = "{0}";
                    break;
                case TermMatch.StartsWith:
                    format = "Starting with '{0}'";
                    break;
                case TermMatch.EndsWith:
                    format = "Ending with '{0}'";
                    break;
                default:
                    throw ExceptionFactory.CreateForUnsupportedEnumValue(match, nameof(match));
            }

            string combinedValues = TermResolver.ToDisplayString(values);

            return string.Format(format, combinedValues);
        }
    }
}
