namespace Atata;

/// <summary>
/// Represents a visitor or rewriter for expression trees.
/// Specifically oriented to handle the expression of function taking a single object argument.
/// </summary>
public class ObjectExpressionStringBuilder : ImprovedExpressionStringBuilder
{
    protected ObjectExpressionStringBuilder(bool isLambdaExpression)
        : base(isLambdaExpression)
    {
    }

    /// <summary>
    /// Outputs a given expression tree to a string.
    /// </summary>
    /// <param name="node">The expression node.</param>
    /// <returns>The string representing the expression.</returns>
    public static new string ExpressionToString(Expression node)
    {
        Guard.ThrowIfNull(node);

        var expressionStringBuilder = new ObjectExpressionStringBuilder(node is LambdaExpression);

        try
        {
            expressionStringBuilder.Visit(node);

            return expressionStringBuilder.CurrentLambda.Body.ToString();
        }
        catch
        {
            return node.ToString();
        }
    }

    protected override Expression VisitMember(MemberExpression node)
    {
        if (node.NodeType == ExpressionType.MemberAccess && node.Expression?.NodeType == ExpressionType.Parameter)
        {
            Out(node.Member.Name);
            return node;
        }
        else
        {
            return base.VisitMember(node);
        }
    }

    protected override Expression VisitMethodCall(MethodCallExpression node)
    {
        if (IsIndexer(node) && node.Object?.NodeType == ExpressionType.Parameter)
        {
            return VisitIndexerAsMethodCall(node);
        }
        else
        {
            bool isExtensionMethod = Attribute.GetCustomAttribute(node.Method, typeof(ExtensionAttribute)) != null;

            if (node.Object?.NodeType == ExpressionType.Parameter || (isExtensionMethod && node.Arguments[0].NodeType == ExpressionType.Parameter))
                return VisitMethodCallOfParameter(node, isExtensionMethod);
        }

        return base.VisitMethodCall(node);
    }

    private MethodCallExpression VisitMethodCallOfParameter(MethodCallExpression node, bool isExtensionMethod)
    {
        Out(node.Method.Name);
        Out("(");

        int firstArgumentIndex = isExtensionMethod ? 1 : 0;

        VisitMethodParameters(node, firstArgumentIndex);

        Out(")");
        return node;
    }
}
