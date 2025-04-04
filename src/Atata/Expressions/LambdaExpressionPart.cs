namespace Atata;

internal class LambdaExpressionPart
{
    internal LambdaExpressionPart()
        : this(null)
    {
    }

    internal LambdaExpressionPart(LambdaExpressionPart? parent) =>
        Parent = parent;

    internal LambdaExpressionPart? Parent { get; }

    internal LiteralExpressionPart Parameters { get; } = new();

    internal BodyExpressionPart Body { get; } = new();

    public override string ToString()
    {
        StringBuilder builder = new();

        string parameters = Parameters.ToString();

        if (parameters.Length > 0)
        {
            builder.Append(Parameters.ToString());
            builder.Append(" => ");
        }

        builder.Append(Body.ToString());
        return builder.ToString();
    }
}
