using System;
using System.Linq;

namespace Atata
{
    public static class QualifierMatchExtensions
    {
        public static string CreateXPathCondition(this QualifierMatch match, string[] values, string operand = ".")
        {
            string operationFormat = match.GetXPathOperationFormat();
            return string.Join(" or ", values.Select(x => string.Format(operationFormat, operand, x)));
        }

        public static string GetXPathOperationFormat(this QualifierMatch match)
        {
            switch (match)
            {
                case QualifierMatch.Contains:
                    return "contains({0}, '{1}')";
                case QualifierMatch.Equals:
                    return "normalize-space({0}) = '{1}'";
                case QualifierMatch.StartsWith:
                    return "starts-with(normalize-space({0}), '{1}')";
                case QualifierMatch.EndsWith:
                    return "substring(normalize-space({0}), string-length(normalize-space({0})) - string-length('{1}') + 1) = '{1}'";
                default:
                    throw new InvalidOperationException("Unsopported Match value.");
            }
        }

        public static Func<string, string, bool> GetPredicate(this QualifierMatch match)
        {
            switch (match)
            {
                case QualifierMatch.Contains:
                    return (text, qualifier) => text.Contains(qualifier);
                case QualifierMatch.Equals:
                    return (text, qualifier) => text.Trim() == qualifier;
                case QualifierMatch.StartsWith:
                    return (text, qualifier) => text.Trim().StartsWith(qualifier);
                case QualifierMatch.EndsWith:
                    return (text, qualifier) => text.Trim().EndsWith(qualifier);
                default:
                    throw new InvalidOperationException("Unsopported Match value.");
            }
        }
    }
}
