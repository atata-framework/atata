namespace Atata;

public static class MethodInfoExtensions
{
    public static Action CompileToLambda(this MethodInfo method, object? instance, params object?[] args)
    {
        if (instance is null)
        {
            return CompileToStaticLambda(method, args);
        }
        else
        {
            var callExpression = method.ToInstanceMethodCallExpression(instance, args);

            var lambda = Expression.Lambda<Action>(callExpression);
            return lambda.Compile();
        }
    }

    public static Func<TResult> CompileToLambda<TResult>(this MethodInfo method, object? instance, params object?[] args)
    {
        if (instance is null)
        {
            return CompileToStaticLambda<TResult>(method, args);
        }
        else
        {
            var callExpression = method.ToInstanceMethodCallExpression(instance, args);

            var lambda = Expression.Lambda<Func<TResult>>(callExpression);
            return lambda.Compile();
        }
    }

    public static Action CompileToStaticLambda(this MethodInfo method, params object?[] args)
    {
        var callExpression = method.ToStaticMethodCallExpression(args);

        var lambda = Expression.Lambda<Action>(callExpression);
        return lambda.Compile();
    }

    public static Func<TResult> CompileToStaticLambda<TResult>(this MethodInfo method, params object?[] args)
    {
        var callExpression = method.ToStaticMethodCallExpression(args);

        var lambda = Expression.Lambda<Func<TResult>>(callExpression);
        return lambda.Compile();
    }

    public static MethodCallExpression ToInstanceMethodCallExpression(this MethodInfo method, object instance, params object?[] args)
    {
        Guard.ThrowIfNull(method);
        Guard.ThrowIfNull(instance);

        var parameterExpressions = args?.Select(Expression.Constant) ?? [];
        return Expression.Call(Expression.Constant(instance), method, parameterExpressions);
    }

    public static MethodCallExpression ToStaticMethodCallExpression(this MethodInfo method, params object?[] args)
    {
        Guard.ThrowIfNull(method);

        var parameterExpressions = args?.Select(Expression.Constant) ?? [];
        return Expression.Call(method, parameterExpressions);
    }
}
