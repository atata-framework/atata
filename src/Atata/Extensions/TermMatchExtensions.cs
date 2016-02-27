using System;
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
                    throw ExceptionsFactory.CreateForUnsupportedEnumValue(match, "match");
            }
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
                    throw ExceptionsFactory.CreateForUnsupportedEnumValue(match, "match");
            }
        }
    }
}
