namespace Atata;

public static class MethodInfoExtensions
{
    public static object InvokeStatic(this MethodInfo method, params object[] args) =>
        method.Invoke(null, args);

    public static TResult InvokeStatic<TResult>(this MethodInfo method, params object[] args) =>
        (TResult)method.Invoke(null, args);

    public static void InvokeAsLambda(this MethodInfo method, object instance, params object[] args)
    {
        if (instance == null)
        {
            InvokeStaticAsLambda(method, args);
        }
        else
        {
            var callExpression = method.ToInstanceMethodCallExpression(instance, args);

            var lambda = Expression.Lambda<Action>(callExpression);
            lambda.Compile().Invoke();
        }
    }

    public static TResult InvokeAsLambda<TResult>(this MethodInfo method, object instance, params object[] args)
    {
        if (instance == null)
        {
            return InvokeStaticAsLambda<TResult>(method, args);
        }
        else
        {
            var callExpression = method.ToInstanceMethodCallExpression(instance, args);

            var lambda = Expression.Lambda<Func<TResult>>(callExpression);
            return lambda.Compile().Invoke();
        }
    }

    public static void InvokeStaticAsLambda(this MethodInfo method, params object[] args)
    {
        var callExpression = method.ToStaticMethodCallExpression(args);

        var lambda = Expression.Lambda<Action>(callExpression);
        lambda.Compile().Invoke();
    }

    public static TResult InvokeStaticAsLambda<TResult>(this MethodInfo method, params object[] args)
    {
        var callExpression = method.ToStaticMethodCallExpression(args);

        var lambda = Expression.Lambda<Func<TResult>>(callExpression);
        return lambda.Compile().Invoke();
    }

    public static MethodCallExpression ToInstanceMethodCallExpression(this MethodInfo method, object instance, params object[] args)
    {
        method.CheckNotNull(nameof(method));
        instance.CheckNotNull(nameof(instance));

        var parameterExpressions = args?.Select(Expression.Constant) ?? [];
        return Expression.Call(Expression.Constant(instance), method, parameterExpressions);
    }

    public static MethodCallExpression ToStaticMethodCallExpression(this MethodInfo method, params object[] args)
    {
        method.CheckNotNull(nameof(method));

        var parameterExpressions = args?.Select(Expression.Constant) ?? [];
        return Expression.Call(method, parameterExpressions);
    }
}
