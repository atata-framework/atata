using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atata
{
    internal class BodyExpressionPart
    {
        private readonly List<object> subParts = new List<object>();

        internal int OperatorAndCount { get; set; }

        internal int OperatorElseCount { get; set; }

        internal LiteralExpressionPart StartNewLiteral()
        {
            LiteralExpressionPart literal = new LiteralExpressionPart();
            subParts.Add(literal);
            return literal;
        }

        internal LambdaExpressionPart StartLambda(LambdaExpressionPart lambda)
        {
            subParts.Add(lambda);
            return lambda;
        }

        public override string ToString()
        {
            string result = subParts.Select(x => x.ToString())
                .Where(x => x.Length > 0)
                .Aggregate(new StringBuilder(), (b, x) => b.Append(x))
                .ToString();

            result = TrimParentheses(result);

            if (OperatorAndCount > 0 && OperatorElseCount == 0)
                return NormalizeParentheses(result, "&&");
            else if (OperatorAndCount == 0 && OperatorElseCount > 0)
                return NormalizeParentheses(result, "||");
            else
                return result;
        }

        private static string NormalizeParentheses(string expression, string conditionalOperator)
        {
            string conditionalOperatorWithSpaces = $" {conditionalOperator} ";

            // TODO: Handle the case when there is " && " or " || " in string variable(s).
            string[] parts = expression.Split(new[] { conditionalOperatorWithSpaces }, StringSplitOptions.RemoveEmptyEntries);

            int expectedStartOpenBraketsCount = parts.Length - 2;
            string expectedExpressionStart = new string(Enumerable.Repeat('(', expectedStartOpenBraketsCount).ToArray());

            if (parts.Length > 2 && parts[0].StartsWith(expectedExpressionStart, StringComparison.Ordinal))
            {
                parts[0] = parts[0].Substring(expectedStartOpenBraketsCount, parts[0].Length - expectedStartOpenBraketsCount);

                for (int i = 1; i < parts.Length - 1; i++)
                {
                    parts[i] = parts[i].Substring(0, parts[i].Length - 1);
                }

                return string.Join(conditionalOperatorWithSpaces, TrimParentheses(parts));
            }
            else if (parts.Length == 2)
            {
                return string.Join(conditionalOperatorWithSpaces, TrimParentheses(parts));
            }
            else
            {
                return expression;
            }
        }

        private static string[] TrimParentheses(string[] expressionParts)
        {
            for (int i = 0; i < expressionParts.Length; i++)
            {
                expressionParts[i] = TrimParentheses(expressionParts[i]);
            }

            return expressionParts;
        }

        private static string TrimParentheses(string expression)
        {
            return expression[0] == '(' && expression[expression.Length - 1] == ')'
                 ? expression.Substring(1, expression.Length - 2)
                 : expression;
        }
    }
}
