#nullable enable

namespace Atata;

/// <summary>
/// Provides a set of extension methods for <see cref="Expression"/>.
/// </summary>
public static class ExpressionExtensions
{
    /// <summary>
    /// Extracts the name of the member.
    /// </summary>
    /// <param name="expression">The expression.</param>
    /// <returns>The name of the member.</returns>
    public static string ExtractMemberName(this Expression expression) =>
        expression.ExtractMember().Name;

    /// <summary>
    /// Extracts the member.
    /// </summary>
    /// <param name="expression">The expression.</param>
    /// <returns>The member information.</returns>
    public static MemberInfo ExtractMember(this Expression expression)
    {
        if (expression is null)
            throw new ArgumentNullException(nameof(expression));

        if (expression is LambdaExpression lambdaExpression)
        {
            return ExtractMember(lambdaExpression.Body);
        }
        else if (expression is MemberExpression memberExpression)
        {
            return memberExpression.Member;
        }
        else if (expression is UnaryExpression unaryExpression)
        {
            return ExtractMember(unaryExpression.Operand);
        }
        else
        {
            throw new ArgumentException($"Inappropriate {nameof(expression)} kind.", nameof(expression));
        }
    }

    internal static (TDelegate Delegate, string TextExpression) ExtractDelegateAndTextExpression<TDelegate>(this Expression<TDelegate> expression) =>
        (expression.Compile(), ObjectExpressionStringBuilder.ExpressionToString(expression));
}
