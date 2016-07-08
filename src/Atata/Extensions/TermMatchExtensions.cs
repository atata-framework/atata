using System;
using System.Collections.Generic;
using System.Linq;

namespace Atata
{
    public static class TermMatchExtensions
    {
        public static string CreateXPathCondition(this TermMatch match, string value, string operand = ".")
        {
            return CreateXPathCondition(match, new[] { value }, operand);
        }

        public static string CreateXPathCondition(this TermMatch match, string[] values, string operand = ".")
        {
            string operationFormat = match.GetXPathOperationFormat();
            return string.Join(" or ", values.Select(x => string.Format(operationFormat, operand, x)));
        }

        public static string GetXPathOperationFormat(this TermMatch match)
        {
            switch (match)
            {
                case TermMatch.Contains:
                    return "contains({0}, '{1}')";
                case TermMatch.Equals:
                    return "normalize-space({0}) = '{1}'";
                case TermMatch.StartsWith:
                    return "starts-with(normalize-space({0}), '{1}')";
                case TermMatch.EndsWith:
                    return "substring(normalize-space({0}), string-length(normalize-space({0})) - string-length('{1}') + 1) = '{1}'";
                default:
                    throw ExceptionFactory.CreateForUnsupportedEnumValue(match, "match");
            }
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
                    return (text, term) => text.Trim().StartsWith(term);
                case TermMatch.EndsWith:
                    return (text, term) => text.Trim().EndsWith(term);
                default:
                    throw ExceptionFactory.CreateForUnsupportedEnumValue(match, "match");
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
                    throw ExceptionFactory.CreateForUnsupportedEnumValue(match, "match");
            }
        }

        public static void Assert(this TermMatch match, IEnumerable<string> expected, string actual, string message, params object[] args)
        {
            switch (match)
            {
                case TermMatch.Contains:
                    ATAssert.ContainsAny(expected, actual, message, args);
                    break;
                case TermMatch.Equals:
                    ATAssert.EqualsAny(expected, actual, message, args);
                    break;
                case TermMatch.StartsWith:
                    ATAssert.StartsWithAny(expected, actual, message, args);
                    break;
                case TermMatch.EndsWith:
                    ATAssert.EndsWithAny(expected, actual, message, args);
                    break;
                default:
                    throw ExceptionFactory.CreateForUnsupportedEnumValue(match, "match");
            }
        }
    }
}
