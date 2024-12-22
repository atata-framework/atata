namespace Atata;

internal class BodyExpressionPart
{
    private readonly List<object> _subParts = [];

    internal int OperatorAndCount { get; set; }

    internal int OperatorElseCount { get; set; }

    internal LiteralExpressionPart StartNewLiteral()
    {
        LiteralExpressionPart literal = new LiteralExpressionPart();
        _subParts.Add(literal);
        return literal;
    }

    internal LambdaExpressionPart StartLambda(LambdaExpressionPart lambda)
    {
        _subParts.Add(lambda);
        return lambda;
    }

    public override string ToString()
    {
        string result = _subParts.Select(x => x.ToString())
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
        string[] parts = expression.Split([conditionalOperatorWithSpaces], StringSplitOptions.RemoveEmptyEntries);

        int expectedStartOpenBraketsCount = parts.Length - 2;
        string expectedExpressionStart = new string(Enumerable.Repeat('(', expectedStartOpenBraketsCount).ToArray());

        if (parts.Length > 2 && parts[0].StartsWith(expectedExpressionStart, StringComparison.Ordinal))
        {
            parts[0] = parts[0][expectedStartOpenBraketsCount..];

            for (int i = 1; i < parts.Length - 1; i++)
            {
                parts[i] = parts[i][..^1];
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

    private static string TrimParentheses(string expression) =>
        expression[0] == '(' && expression[^1] == ')'
            ? expression[1..^1]
            : expression;
}
