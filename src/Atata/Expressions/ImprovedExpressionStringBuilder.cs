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
        new EnumExpressionValueStringifier(),
        new DecimalExpressionValueStringifier(),
        new TypeExpressionValueStringifier()
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

    protected override void Out(string? s) =>
        CurrentLiteral.Append(s);

    protected override void Out(char c) =>
        CurrentLiteral.Append(c);

    protected override void OutType(Type type) =>
        CurrentLiteral.Append(type.ToStringInShortForm());

    private static bool CanStringifyValue(Type valueType)
    {
        Type underlyingType = Nullable.GetUnderlyingType(valueType) ?? valueType;

        return s_expressionValueStringifiers.Any(x => x.CanHandle(underlyingType));
    }

    private static bool TryStringifyValue(object? value, Type valueType, [NotNullWhen(true)] out string? result)
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

    [SuppressMessage("Critical Code Smell", "S134:Control flow statements \"if\", \"switch\", \"for\", \"foreach\", \"while\", \"do\"  and \"try\" should not be nested too deeply")]
    protected override Expression VisitMember(MemberExpression node)
    {
        if (node.Member is FieldInfo field && !DoesMemberExpressionHasParameterRoot(node))
        {
            if (CanStringifyValue(node.Type))
            {
                try
                {
                    object? value = Expression.Lambda(node).Compile().DynamicInvoke();

                    if (TryStringifyValue(value, node.Type, out string? valueAsString))
                        Out(valueAsString);

                    return node;
                }
                catch (Exception exception)
                {
                    // Just proceed to base member visit.
                    Debug.Fail(exception.ToString());
                }
            }
            else if (field.IsStatic && !field.IsPrivate)
            {
                OutType(field.DeclaringType!);
                Out('.');
                Out(node.Member.Name);
                return node;
            }
            else
            {
                Out(node.Member.Name);
                return node;
            }
        }

        return base.VisitMember(node);
    }

    private static bool DoesMemberExpressionHasParameterRoot(MemberExpression node) =>
        node.Expression is MemberExpression parentMember
            ? DoesMemberExpressionHasParameterRoot(parentMember)
            : node.Expression is ParameterExpression;

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
            OutType(node.Method.DeclaringType!);
            Out('.');
        }
        else if (IsIndexer(node))
        {
            if (node.Object != null)
                Visit(node.Object);

            return VisitIndexerAsMethodCall(node);
        }

        int start = 0;
        Expression? ob = node.Object;

        if (isExtensionMethod)
        {
            start = 1;
            ob = node.Arguments[0];
        }

        if (ob != null)
        {
            Visit(ob);
            Out('.');
        }

        Out(node.Method.Name);

        if (node.Method.IsGenericMethod && !isExtensionMethod)
        {
            Type[] genericArguments = node.Method.GetGenericArguments();
            Out('<');

            for (int i = 0; i < genericArguments.Length; i++)
            {
                if (i != 0)
                    Out(", ");

                OutType(genericArguments[i]);
            }

            Out('>');
        }

        Out('(');
        VisitMethodParameters(node, start);
        Out(')');

        return node;
    }

    protected static bool IsIndexer(MethodCallExpression node) =>
        node.Method.IsSpecialName && (node.Method.Name == "get_Item" || node.Method.Name == "get_Chars") && node.Arguments.Count > 0;

    protected Expression VisitIndexerAsMethodCall(MethodCallExpression node)
    {
        Out('[');

        for (int i = 0; i < node.Arguments.Count; i++)
        {
            if (i > 0)
                Out(", ");

            Visit(node.Arguments[i]);
        }

        Out(']');
        return node;
    }

    protected override void VisitMethodParameters(MethodCallExpression node, int start)
    {
        ParameterInfo[] methodParameters = node.Method.GetParameters();

        for (int i = start; i < node.Arguments.Count; i++)
        {
            if (i > start)
                Out(", ");

            ParameterInfo parameter = methodParameters[i];

            VisitMethodParameter(parameter, node.Arguments[i]);
        }
    }

    private void VisitMethodParameter(ParameterInfo parameter, Expression argumentExpression)
    {
        if (argumentExpression is MemberExpression memberExpression && memberExpression.Member is FieldInfo)
        {
            if (parameter.IsOut)
            {
                Out($"out {memberExpression.Member.Name}");
            }
            else if (parameter.ParameterType.IsByRef)
            {
                Out($"ref {memberExpression.Member.Name}");
            }
            else
            {
                Visit(argumentExpression);
            }
        }
        else if (argumentExpression is ParameterExpression parameterExpression && parameter.ParameterType.IsByRef)
        {
            Out($"ref {parameterExpression.Name}");
        }
        else
        {
            Visit(argumentExpression);
        }
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
        Out("new ");
        OutType(node.Type);

        bool addParentheses = alwaysAddParentheses || node.Arguments.Count > 0;

        if (addParentheses)
            Out('(');

        OutArguments(node.Arguments, node.Members);

        if (addParentheses)
            Out(')');

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

    private void OutArguments(ReadOnlyCollection<Expression> argumentExpressions, ReadOnlyCollection<MemberInfo>? members)
    {
        for (int i = 0; i < argumentExpressions.Count; i++)
        {
            if (i > 0)
                Out(", ");

            if (members?.Count > i)
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
        UnaryExpression unaryExpression = (node.Left as UnaryExpression) ?? (node.Right as UnaryExpression)
            ?? throw new ArgumentException("Invalid node. Expected Left or Right to be UnaryExpression.", nameof(node));

        Type enumType = unaryExpression.Operand.Type;

        return VisitComparisonWithConvert(
            node,
            x => ((Enum)Enum.ToObject(enumType, x)).ToExpressionValueString(wrapCombinationalValueWithParentheses: true));
    }

    private BinaryExpression VisitComparisonWithConvert(BinaryExpression node, Func<object, string> valueConverter)
    {
        void OutPart(Expression part)
        {
            if (part is ConstantExpression { Value: not null } constantExpression)
                Out(valueConverter.Invoke(constantExpression.Value));
            else
                Visit(part);
        }

        Out('(');
        OutPart(node.Left);
        Out(' ');
        Out(GetBinaryOperator(node));
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
        // TryStringifyValue covers all possible constant non-default values including null.
        if (!TryStringifyValue(node.Value, node.Type, out string? valueAsString))
            valueAsString = "default";

        Out(valueAsString);
        return node;
    }

    public override string ToString() =>
        CurrentLambda.ToString();
}
