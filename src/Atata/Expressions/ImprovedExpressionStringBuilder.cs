namespace Atata;

/// <summary>
/// Represents an improved version of visitor or rewriter for expression trees.
/// </summary>
public class ImprovedExpressionStringBuilder : ExpressionStringBuilder
{
    private static readonly IExpressionValueStringifier[] s_expressionValueStringifiers =
    [
        new StringExpressionValueStringifier(),
        new BoolExpressionValueStringifier(),
        new CharExpressionValueStringifier(),
        new PrimitiveExpressionValueStringifier(),
        new EnumExpressionValueStringifier()
    ];

    private bool _expectLambdaVisit;

    protected ImprovedExpressionStringBuilder(bool isLambdaExpression)
    {
        if (isLambdaExpression)
        {
            _expectLambdaVisit = true;
            CurrentLiteral = CurrentLambda.Parameters;
        }
        else
        {
            CurrentLiteral = CurrentLambda.Body.StartNewLiteral();
        }
    }

    private protected LambdaExpressionPart CurrentLambda { get; private set; } = new LambdaExpressionPart();

    private protected LiteralExpressionPart CurrentLiteral { get; private set; }

    /// <summary>
    /// Outputs a given expression tree to a string.
    /// </summary>
    /// <param name="node">The expression node.</param>
    /// <returns>The string representing the expression.</returns>
    public static new string ExpressionToString(Expression node)
    {
        Guard.ThrowIfNull(node);

        ImprovedExpressionStringBuilder expressionStringBuilder = new(node is LambdaExpression);

        try
        {
            expressionStringBuilder.Visit(node);

            return expressionStringBuilder.ToString();
        }
        catch
        {
            return node.ToString();
        }
    }

    protected override void Out(string s) =>
        CurrentLiteral.Append(s);

    protected override void Out(char c) =>
        CurrentLiteral.Append(c);

    private static bool CanStringifyValue(Type valueType)
    {
        Type underlyingType = Nullable.GetUnderlyingType(valueType) ?? valueType;

        return s_expressionValueStringifiers.Any(x => x.CanHandle(underlyingType));
    }

    private static bool TryStringifyValue(object value, Type valueType, [NotNullWhen(true)] out string? result)
    {
        if (value is null)
        {
            result = "null";
            return true;
        }

        Type underlyingType = Nullable.GetUnderlyingType(valueType) ?? valueType;

        var stringifier = s_expressionValueStringifiers.FirstOrDefault(x => x.CanHandle(underlyingType));

        if (stringifier != null)
        {
            try
            {
                result = stringifier.Handle(value);
                return true;
            }
            catch
            {
                // Do nothing here, just return false.
            }
        }

        result = null;
        return false;
    }

    protected override Expression VisitLambda<T>(Expression<T> node)
    {
        if (_expectLambdaVisit)
        {
            _expectLambdaVisit = false;
        }
        else
        {
            LambdaExpressionPart newLambda = new LambdaExpressionPart(CurrentLambda);
            CurrentLambda.Body.StartLambda(newLambda);
            CurrentLiteral = newLambda.Parameters;
            CurrentLambda = newLambda;
        }

        if (node.Parameters.Count == 1)
            Visit(node.Parameters[0]);
        else
            VisitExpressions('(', node.Parameters, ')');

        CurrentLiteral = CurrentLambda.Body.StartNewLiteral();

        Visit(node.Body);

        if (CurrentLambda.Parent != null)
        {
            CurrentLambda = CurrentLambda.Parent;
            CurrentLiteral = CurrentLambda.Body.StartNewLiteral();
        }

        return node;
    }

    protected override Expression VisitMember(MemberExpression node)
    {
        if (node.Member is FieldInfo)
        {
            if (CanStringifyValue(node.Type))
            {
                object value = Expression.Lambda(node).Compile().DynamicInvoke();

                if (TryStringifyValue(value, node.Type, out string? valueAsString))
                {
                    Out(valueAsString);
                    return node;
                }
            }

            Out(node.Member.Name);
            return node;
        }

        return base.VisitMember(node);
    }

    private static bool IsParameterExpression(Expression expression) =>
        expression switch
        {
            ParameterExpression => true,
            MemberExpression memberExpression => IsParameterExpression(memberExpression.Expression),
            _ => false
        };

    protected override Expression VisitMemberInit(MemberInitExpression node)
    {
        VisitNewKnownType(node.NewExpression, alwaysAddParentheses: false);

        Out(" { ");

        for (int i = 0, n = node.Bindings.Count; i < n; i++)
        {
            if (i > 0)
                Out(", ");

            VisitMemberBinding(node.Bindings[i]);
        }

        Out(" }");

        return node;
    }

    protected override Expression VisitMethodCall(MethodCallExpression node)
    {
        bool isExtensionMethod = Attribute.GetCustomAttribute(node.Method, typeof(ExtensionAttribute)) != null;

        if (node.Method.IsStatic && !isExtensionMethod && node.Method.DeclaringType != typeof(object))
        {
            OutStaticClass(node.Method.DeclaringType);
        }
        else if (IsIndexer(node))
        {
            if (node.Object != null)
                Visit(node.Object);

            return VisitIndexerAsMethodCall(node);
        }

        return base.VisitMethodCall(node);
    }

    protected static bool IsIndexer(MethodCallExpression node) =>
        node.Method.IsSpecialName && (node.Method.Name == "get_Item" || node.Method.Name == "get_Chars") && node.Arguments.Count > 0;

    protected Expression VisitIndexerAsMethodCall(MethodCallExpression node)
    {
        Out("[");

        for (int i = 0; i < node.Arguments.Count; i++)
        {
            if (i > 0)
                Out(", ");

            Visit(node.Arguments[i]);
        }

        Out("]");
        return node;
    }

    private void OutStaticClass(Type type)
    {
        if (type.DeclaringType != null)
            OutStaticClass(type.DeclaringType);

        Out(type.Name);
        Out(".");
    }

    protected override Expression VisitMethodParameters(MethodCallExpression node, int startArgumentIndex)
    {
        ParameterInfo[] methodParameters = node.Method.GetParameters();

        for (int i = startArgumentIndex; i < node.Arguments.Count; i++)
        {
            if (i > startArgumentIndex)
                Out(", ");

            ParameterInfo parameter = methodParameters[i];

            VisitMethodParameter(parameter, node.Arguments[i]);
        }

        return node;
    }

    private void VisitMethodParameter(ParameterInfo parameter, Expression argumentExpression)
    {
        if (argumentExpression is MemberExpression memberExpression && memberExpression.Member is FieldInfo)
        {
            if (parameter.IsOut)
            {
                Out($"out {memberExpression.Member.Name}");
                return;
            }
            else if (parameter.ParameterType.IsByRef)
            {
                Out($"ref {memberExpression.Member.Name}");
                return;
            }
        }

        Visit(argumentExpression);
    }

    protected override Expression VisitNewArray(NewArrayExpression node)
    {
        if (node.NodeType == ExpressionType.NewArrayInit)
        {
            VisitExpressions('[', node.Expressions, ']');
            return node;
        }

        return base.VisitNewArray(node);
    }

    protected override Expression VisitNew(NewExpression node) =>
        node.Type.Name.StartsWith("<>", StringComparison.Ordinal)
            ? VisitNewAnonymousType(node)
            : VisitNewKnownType(node);

    private NewExpression VisitNewKnownType(NewExpression node, bool alwaysAddParentheses = true)
    {
        Out("new " + node.Type.Name);

        bool addParentheses = alwaysAddParentheses || node.Arguments.Count > 0;

        if (addParentheses)
            Out("(");

        OutArguments(node.Arguments, node.Members);

        if (addParentheses)
            Out(")");

        return node;
    }

    private NewExpression VisitNewAnonymousType(NewExpression node)
    {
        Out("new { ");

        if (node.Arguments.Count > 0)
        {
            OutArguments(node.Arguments, node.Members);

            Out(" ");
        }

        Out("}");
        return node;
    }

    private void OutArguments(ReadOnlyCollection<Expression> argumentExpressions, ReadOnlyCollection<MemberInfo> members)
    {
        for (int i = 0; i < argumentExpressions.Count; i++)
        {
            if (i > 0)
                Out(", ");

            if (members != null)
            {
                Out(members[i].Name);
                Out(" = ");
            }

            Visit(argumentExpressions[i]);
        }
    }

    protected override Expression VisitBinary(BinaryExpression node)
    {
        if (node.NodeType == ExpressionType.AndAlso)
            CurrentLambda.Body.OperatorAndCount++;

        if (node.NodeType == ExpressionType.OrElse)
            CurrentLambda.Body.OperatorElseCount++;

        if (IsEnumComparison(node))
            return VisitEnumComparison(node);

        if (IsCharComparison(node))
            return VisitComparisonWithConvert(node, x => $"'{Convert.ToChar(x)}'");

        return base.VisitBinary(node);
    }

    private static bool IsCharComparison(BinaryExpression node) =>
        node.NodeType != ExpressionType.ArrayIndex && (IsCharComparison(node.Left, node.Right) || IsCharComparison(node.Right, node.Left));

    private static bool IsCharComparison(Expression left, Expression right) =>
        left.NodeType == ExpressionType.Convert
        && left.Type == typeof(int)
        && (left as UnaryExpression)?.Operand?.Type == typeof(char)
        && right.NodeType == ExpressionType.Constant
        && right.Type == typeof(int);

    private static bool IsEnumComparison(BinaryExpression node) =>
        node.NodeType != ExpressionType.ArrayIndex && (IsEnumComparison(node.Left, node.Right) || IsEnumComparison(node.Right, node.Left));

    private static bool IsEnumComparison(Expression left, Expression right) =>
        left.NodeType == ExpressionType.Convert
        && left.Type.IsPrimitive
        && left.Type == right.Type
        && ((left as UnaryExpression)?.Operand.Type.IsEnum ?? false)
        && right.NodeType == ExpressionType.Constant;

    private BinaryExpression VisitEnumComparison(BinaryExpression node)
    {
        Type? enumType = ((node.Left as UnaryExpression) ?? (node.Right as UnaryExpression))?.Operand.Type;

        return VisitComparisonWithConvert(
            node,
            x => ((Enum)Enum.ToObject(enumType, x)).ToExpressionValueString(wrapCombinationalValueWithParentheses: true));
    }

    private BinaryExpression VisitComparisonWithConvert(BinaryExpression node, Func<object, string> valueConverter)
    {
        void OutPart(Expression part)
        {
            if (part is ConstantExpression constantExpression)
                Out(valueConverter.Invoke(constantExpression.Value));
            else
                Visit(part);
        }

        Out('(');
        OutPart(node.Left);
        Out(' ');
        Out(GetBinaryOperator(node.NodeType));
        Out(' ');
        OutPart(node.Right);
        Out(')');

        return node;
    }

    protected override Expression VisitUnary(UnaryExpression node)
    {
        switch (node.NodeType)
        {
            case ExpressionType.Convert:
            case ExpressionType.ConvertChecked:
                Visit(node.Operand);
                return node;
            default:
                return base.VisitUnary(node);
        }
    }

    protected override Expression VisitConstant(ConstantExpression node)
    {
        if (TryStringifyValue(node.Value, node.Type, out string? valueAsString))
        {
            Out(valueAsString);
            return node;
        }

        return base.VisitConstant(node);
    }

    public override string ToString() =>
        CurrentLambda.ToString();
}
