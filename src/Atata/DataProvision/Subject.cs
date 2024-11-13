namespace Atata;

/// <summary>
/// Provides a set of static methods for testing static class methods and properties.
/// </summary>
public static class Subject
{
    internal const string ResultNameEnding = " => result";

    internal const string ExceptionNameEnding = " => exception";

    /// <summary>
    /// Gets or sets the default name of the subject.
    /// The default value is <c>"subject"</c>.
    /// </summary>
    public static string DefaultSubjectName { get; set; } = "subject";

    /// <inheritdoc cref="SubjectBase{TObject, TSubject}.ResultOf{TResult}(Expression{Func{TObject, TResult}})"/>
    public static Subject<TResult> ResultOf<TResult>(Expression<Func<TResult>> functionExpression)
    {
        functionExpression.CheckNotNull(nameof(functionExpression));

        var (function, functionName) = functionExpression.ExtractDelegateAndTextExpression();

        return ResultOf(function, functionName);
    }

    /// <inheritdoc cref="SubjectBase{TObject, TSubject}.ResultOf{TResult}(Func{TObject, TResult}, string)"/>
    public static Subject<TResult> ResultOf<TResult>(Func<TResult> function, string functionName) =>
        SubjectOf(function, BuildResultName(functionName));

    /// <inheritdoc cref="SubjectBase{TObject, TSubject}.ResultOf{TResult}(Expression{Func{TObject, TResult}})"/>
    public static Subject<TResult> ResultOf<TResult>(Expression<Func<ValueTask<TResult>>> functionExpression)
    {
        functionExpression.CheckNotNull(nameof(functionExpression));

        var (function, functionName) = functionExpression.ExtractDelegateAndTextExpression();

        return ResultOf(function, functionName);
    }

    /// <inheritdoc cref="SubjectBase{TObject, TSubject}.ResultOf{TResult}(Func{TObject, TResult}, string)"/>
    public static Subject<TResult> ResultOf<TResult>(Func<ValueTask<TResult>> function, string functionName) =>
        SubjectOf(function, BuildResultName(functionName));

    /// <inheritdoc cref="SubjectBase{TObject, TSubject}.ResultOf{TResult}(Expression{Func{TObject, TResult}})"/>
    public static Subject<TResult> ResultOf<TResult>(Expression<Func<Task<TResult>>> functionExpression)
    {
        functionExpression.CheckNotNull(nameof(functionExpression));

        var (function, functionName) = functionExpression.ExtractDelegateAndTextExpression();

        return ResultOf(function, functionName);
    }

    /// <inheritdoc cref="SubjectBase{TObject, TSubject}.ResultOf{TResult}(Func{TObject, TResult}, string)"/>
    public static Subject<TResult> ResultOf<TResult>(Func<Task<TResult>> function, string functionName) =>
        SubjectOf(function, BuildResultName(functionName));

    /// <inheritdoc cref="SubjectBase{TObject, TSubject}.SubjectOf{TResult}(Expression{Func{TObject, TResult}})"/>
    public static Subject<TResult> SubjectOf<TResult>(Expression<Func<TResult>> functionExpression)
    {
        functionExpression.CheckNotNull(nameof(functionExpression));

        var (function, functionName) = functionExpression.ExtractDelegateAndTextExpression();

        return SubjectOf(function, functionName);
    }

    /// <inheritdoc cref="SubjectBase{TObject, TSubject}.SubjectOf{TResult}(Func{TObject, TResult}, string)"/>
    public static Subject<TResult> SubjectOf<TResult>(Func<TResult> function, string functionName)
    {
        function.CheckNotNull(nameof(function));
        functionName.CheckNotNull(nameof(functionName));

        return new(
            new LazyObjectSource<TResult>(function),
            functionName);
    }

    /// <inheritdoc cref="SubjectBase{TObject, TSubject}.SubjectOf{TResult}(Expression{Func{TObject, TResult}})"/>
    public static Subject<TResult> SubjectOf<TResult>(Expression<Func<ValueTask<TResult>>> functionExpression)
    {
        functionExpression.CheckNotNull(nameof(functionExpression));

        var (function, functionName) = functionExpression.ExtractDelegateAndTextExpression();

        return SubjectOf(function, functionName);
    }

    /// <inheritdoc cref="SubjectBase{TObject, TSubject}.SubjectOf{TResult}(Func{TObject, TResult}, string)"/>
    public static Subject<TResult> SubjectOf<TResult>(Func<ValueTask<TResult>> function, string functionName)
    {
        function.CheckNotNull(nameof(function));
        functionName.CheckNotNull(nameof(functionName));

        return new(
            new LazyObjectSource<TResult>(() => function.Invoke().RunSync()),
            functionName);
    }

    /// <inheritdoc cref="SubjectBase{TObject, TSubject}.SubjectOf{TResult}(Expression{Func{TObject, TResult}})"/>
    public static Subject<TResult> SubjectOf<TResult>(Expression<Func<Task<TResult>>> functionExpression)
    {
        functionExpression.CheckNotNull(nameof(functionExpression));

        var (function, functionName) = functionExpression.ExtractDelegateAndTextExpression();

        return SubjectOf(function, functionName);
    }

    /// <inheritdoc cref="SubjectBase{TObject, TSubject}.SubjectOf{TResult}(Func{TObject, TResult}, string)"/>
    public static Subject<TResult> SubjectOf<TResult>(Func<Task<TResult>> function, string functionName)
    {
        function.CheckNotNull(nameof(function));
        functionName.CheckNotNull(nameof(functionName));

        return new(
            new LazyObjectSource<TResult>(() => function.Invoke().RunSync()),
            functionName);
    }

    /// <inheritdoc cref="SubjectBase{TObject, TSubject}.DynamicResultOf{TResult}(Expression{Func{TObject, TResult}})"/>
    public static Subject<TResult> DynamicResultOf<TResult>(Expression<Func<TResult>> functionExpression)
    {
        functionExpression.CheckNotNull(nameof(functionExpression));

        var (function, functionName) = functionExpression.ExtractDelegateAndTextExpression();

        return DynamicResultOf(function, functionName);
    }

    /// <inheritdoc cref="SubjectBase{TObject, TSubject}.DynamicResultOf{TResult}(Func{TObject, TResult}, string)"/>
    public static Subject<TResult> DynamicResultOf<TResult>(Func<TResult> function, string functionName) =>
        DynamicSubjectOf(function, BuildResultName(functionName));

    /// <inheritdoc cref="SubjectBase{TObject, TSubject}.DynamicResultOf{TResult}(Expression{Func{TObject, TResult}})"/>
    public static Subject<TResult> DynamicResultOf<TResult>(Expression<Func<ValueTask<TResult>>> functionExpression)
    {
        functionExpression.CheckNotNull(nameof(functionExpression));

        var (function, functionName) = functionExpression.ExtractDelegateAndTextExpression();

        return DynamicResultOf(function, functionName);
    }

    /// <inheritdoc cref="SubjectBase{TObject, TSubject}.DynamicResultOf{TResult}(Func{TObject, TResult}, string)"/>
    public static Subject<TResult> DynamicResultOf<TResult>(Func<ValueTask<TResult>> function, string functionName) =>
        DynamicSubjectOf(function, BuildResultName(functionName));

    /// <inheritdoc cref="SubjectBase{TObject, TSubject}.DynamicSubjectOf{TResult}(Expression{Func{TObject, TResult}})"/>
    public static Subject<TResult> DynamicSubjectOf<TResult>(Expression<Func<TResult>> functionExpression)
    {
        functionExpression.CheckNotNull(nameof(functionExpression));

        var (function, functionName) = functionExpression.ExtractDelegateAndTextExpression();

        return DynamicSubjectOf(function, functionName);
    }

    /// <inheritdoc cref="SubjectBase{TObject, TSubject}.DynamicSubjectOf{TResult}(Func{TObject, TResult}, string)"/>
    public static Subject<TResult> DynamicSubjectOf<TResult>(Func<TResult> function, string functionName)
    {
        function.CheckNotNull(nameof(function));
        functionName.CheckNotNull(nameof(functionName));

        return new(
            DynamicObjectSource.Create(function),
            functionName);
    }

    /// <inheritdoc cref="SubjectBase{TObject, TSubject}.DynamicSubjectOf{TResult}(Expression{Func{TObject, TResult}})"/>
    public static Subject<TResult> DynamicSubjectOf<TResult>(Expression<Func<ValueTask<TResult>>> functionExpression)
    {
        functionExpression.CheckNotNull(nameof(functionExpression));

        var (function, functionName) = functionExpression.ExtractDelegateAndTextExpression();

        return DynamicSubjectOf(function, functionName);
    }

    /// <inheritdoc cref="SubjectBase{TObject, TSubject}.DynamicSubjectOf{TResult}(Func{TObject, TResult}, string)"/>
    public static Subject<TResult> DynamicSubjectOf<TResult>(Func<ValueTask<TResult>> function, string functionName)
    {
        function.CheckNotNull(nameof(function));
        functionName.CheckNotNull(nameof(functionName));

        return new(
            DynamicObjectSource.Create(() => function.Invoke().RunSync()),
            functionName);
    }

    /// <inheritdoc cref="SubjectBase{TObject, TSubject}.DynamicSubjectOf{TResult}(Expression{Func{TObject, TResult}})"/>
    public static Subject<TResult> DynamicSubjectOf<TResult>(Expression<Func<Task<TResult>>> functionExpression)
    {
        functionExpression.CheckNotNull(nameof(functionExpression));

        var (function, functionName) = functionExpression.ExtractDelegateAndTextExpression();

        return DynamicSubjectOf(function, functionName);
    }

    /// <inheritdoc cref="SubjectBase{TObject, TSubject}.DynamicSubjectOf{TResult}(Func{TObject, TResult}, string)"/>
    public static Subject<TResult> DynamicSubjectOf<TResult>(Func<Task<TResult>> function, string functionName)
    {
        function.CheckNotNull(nameof(function));
        functionName.CheckNotNull(nameof(functionName));

        return new(
            DynamicObjectSource.Create(() => function.Invoke().RunSync()),
            functionName);
    }

    /// <summary>
    /// Creates a new lazy <see cref="ActionProvider"/> from the invocation of the specified <paramref name="actionExpression"/>.
    /// </summary>
    /// <param name="actionExpression">The action expression.</param>
    /// <returns>A new <see cref="ActionProvider"/> instance.</returns>
    public static ActionProvider Invoking(Expression<Action> actionExpression)
    {
        actionExpression.CheckNotNull(nameof(actionExpression));

        var (action, actionName) = actionExpression.ExtractDelegateAndTextExpression();

        return Invoking(action, actionName);
    }

    /// <summary>
    /// Creates a new lazy <see cref="ActionProvider"/> from the invocation of the specified <paramref name="action"/>
    /// with the specified <paramref name="actionName"/>.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <param name="actionName">Name of the action.</param>
    /// <returns>A new <see cref="ActionProvider"/> instance.</returns>
    public static ActionProvider Invoking(Action action, string actionName)
    {
        action.CheckNotNull(nameof(action));
        actionName.CheckNotNull(nameof(actionName));

        return new(
            new LazyObjectSource<Action>(() => action),
            actionName);
    }

    /// <summary>
    /// Creates a new lazy <see cref="ActionProvider"/> from the invocation of the specified <paramref name="functionExpression"/>.
    /// </summary>
    /// <param name="functionExpression">The function expression.</param>
    /// <returns>A new <see cref="ActionProvider"/> instance.</returns>
    public static ActionProvider Invoking(Expression<Func<ValueTask>> functionExpression)
    {
        functionExpression.CheckNotNull(nameof(functionExpression));

        var (function, functionName) = functionExpression.ExtractDelegateAndTextExpression();

        return Invoking(function, functionName);
    }

    /// <summary>
    /// Creates a new lazy <see cref="ActionProvider"/> from the invocation of the specified <paramref name="function"/>
    /// with the specified <paramref name="functionName"/>.
    /// </summary>
    /// <param name="function">The function.</param>
    /// <param name="functionName">Name of the function.</param>
    /// <returns>A new <see cref="ActionProvider"/> instance.</returns>
    public static ActionProvider Invoking(Func<ValueTask> function, string functionName)
    {
        function.CheckNotNull(nameof(function));
        functionName.CheckNotNull(nameof(functionName));

        return new(
            new LazyObjectSource<Action>(() => () => function.Invoke().RunSync()),
            functionName);
    }

    /// <summary>
    /// Creates a new lazy <see cref="ActionProvider"/> from the invocation of the specified <paramref name="functionExpression"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of the value task result.</typeparam>
    /// <param name="functionExpression">The function expression.</param>
    /// <returns>A new <see cref="ActionProvider"/> instance.</returns>
    public static ActionProvider Invoking<TResult>(Expression<Func<ValueTask<TResult>>> functionExpression)
    {
        functionExpression.CheckNotNull(nameof(functionExpression));

        var (function, functionName) = functionExpression.ExtractDelegateAndTextExpression();

        return Invoking(function, functionName);
    }

    /// <summary>
    /// Creates a new lazy <see cref="ActionProvider"/> from the invocation of the specified <paramref name="function"/>
    /// with the specified <paramref name="functionName"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of the value task result.</typeparam>
    /// <param name="function">The function.</param>
    /// <param name="functionName">Name of the function.</param>
    /// <returns>A new <see cref="ActionProvider"/> instance.</returns>
    public static ActionProvider Invoking<TResult>(Func<ValueTask<TResult>> function, string functionName)
    {
        function.CheckNotNull(nameof(function));
        functionName.CheckNotNull(nameof(functionName));

        return new(
            new LazyObjectSource<Action>(() => () => function.Invoke().RunSync()),
            functionName);
    }

    /// <summary>
    /// Creates a new lazy <see cref="ActionProvider"/> from the invocation of the specified <paramref name="functionExpression"/>.
    /// </summary>
    /// <param name="functionExpression">The function expression.</param>
    /// <returns>A new <see cref="ActionProvider"/> instance.</returns>
    public static ActionProvider Invoking(Expression<Func<Task>> functionExpression)
    {
        functionExpression.CheckNotNull(nameof(functionExpression));

        var (function, functionName) = functionExpression.ExtractDelegateAndTextExpression();

        return Invoking(function, functionName);
    }

    /// <summary>
    /// Creates a new lazy <see cref="ActionProvider"/> from the invocation of the specified <paramref name="function"/>
    /// with the specified <paramref name="functionName"/>.
    /// </summary>
    /// <param name="function">The function.</param>
    /// <param name="functionName">Name of the function.</param>
    /// <returns>A new <see cref="ActionProvider"/> instance.</returns>
    public static ActionProvider Invoking(Func<Task> function, string functionName)
    {
        function.CheckNotNull(nameof(function));
        functionName.CheckNotNull(nameof(functionName));

        return new(
            new LazyObjectSource<Action>(() => function.Invoke().RunSync),
            functionName);
    }

    /// <summary>
    /// Creates a new lazy <see cref="ActionProvider"/> from the invocation of the specified <paramref name="functionExpression"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of the task result.</typeparam>
    /// <param name="functionExpression">The function expression.</param>
    /// <returns>A new <see cref="ActionProvider"/> instance.</returns>
    public static ActionProvider Invoking<TResult>(Expression<Func<Task<TResult>>> functionExpression)
    {
        functionExpression.CheckNotNull(nameof(functionExpression));

        var (function, functionName) = functionExpression.ExtractDelegateAndTextExpression();

        return Invoking(function, functionName);
    }

    /// <summary>
    /// Creates a new lazy <see cref="ActionProvider"/> from the invocation of the specified <paramref name="function"/>
    /// with the specified <paramref name="functionName"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of the task result.</typeparam>
    /// <param name="function">The function.</param>
    /// <param name="functionName">Name of the function.</param>
    /// <returns>A new <see cref="ActionProvider"/> instance.</returns>
    public static ActionProvider Invoking<TResult>(Func<Task<TResult>> function, string functionName)
    {
        function.CheckNotNull(nameof(function));
        functionName.CheckNotNull(nameof(functionName));

        return new(
            new LazyObjectSource<Action>(() => function.Invoke().RunSync),
            functionName);
    }

    /// <summary>
    /// Creates a new lazy <see cref="ActionProvider"/> from the invocation of the specified <paramref name="actionExpression"/>.
    /// </summary>
    /// <param name="actionExpression">The action expression.</param>
    /// <returns>A new <see cref="ActionProvider"/> instance.</returns>
    public static ActionProvider DynamicInvoking(Expression<Action> actionExpression)
    {
        actionExpression.CheckNotNull(nameof(actionExpression));

        var (action, actionName) = actionExpression.ExtractDelegateAndTextExpression();

        return DynamicInvoking(action, actionName);
    }

    /// <summary>
    /// Creates a new dynamic <see cref="ActionProvider"/> from the invocation of the specified <paramref name="action"/>
    /// with the specified <paramref name="actionName"/>.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <param name="actionName">Name of the action.</param>
    /// <returns>A new <see cref="ActionProvider"/> instance.</returns>
    public static ActionProvider DynamicInvoking(Action action, string actionName)
    {
        action.CheckNotNull(nameof(action));
        actionName.CheckNotNull(nameof(actionName));

        return new(
            DynamicObjectSource.Create(() => action),
            actionName);
    }

    /// <summary>
    /// Creates a new lazy <see cref="ActionProvider"/> from the invocation of the specified <paramref name="functionExpression"/>.
    /// </summary>
    /// <param name="functionExpression">The function expression.</param>
    /// <returns>A new <see cref="ActionProvider"/> instance.</returns>
    public static ActionProvider DynamicInvoking(Expression<Func<ValueTask>> functionExpression)
    {
        functionExpression.CheckNotNull(nameof(functionExpression));

        var (function, functionName) = functionExpression.ExtractDelegateAndTextExpression();

        return DynamicInvoking(function, functionName);
    }

    /// <summary>
    /// Creates a new dynamic <see cref="ActionProvider"/> from the invocation of the specified <paramref name="function"/>
    /// with the specified <paramref name="functionName"/>.
    /// </summary>
    /// <param name="function">The function.</param>
    /// <param name="functionName">Name of the function.</param>
    /// <returns>A new <see cref="ActionProvider"/> instance.</returns>
    public static ActionProvider DynamicInvoking(Func<ValueTask> function, string functionName)
    {
        function.CheckNotNull(nameof(function));
        functionName.CheckNotNull(nameof(functionName));

        return new(
            new DynamicObjectSource<Action>(() => () => function.Invoke().RunSync()),
            functionName);
    }

    /// <summary>
    /// Creates a new lazy <see cref="ActionProvider"/> from the invocation of the specified <paramref name="functionExpression"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of the value task result.</typeparam>
    /// <param name="functionExpression">The function expression.</param>
    /// <returns>A new <see cref="ActionProvider"/> instance.</returns>
    public static ActionProvider DynamicInvoking<TResult>(Expression<Func<ValueTask<TResult>>> functionExpression)
    {
        functionExpression.CheckNotNull(nameof(functionExpression));

        var (function, functionName) = functionExpression.ExtractDelegateAndTextExpression();

        return DynamicInvoking(function, functionName);
    }

    /// <summary>
    /// Creates a new dynamic <see cref="ActionProvider"/> from the invocation of the specified <paramref name="function"/>
    /// with the specified <paramref name="functionName"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of the value task result.</typeparam>
    /// <param name="function">The function.</param>
    /// <param name="functionName">Name of the function.</param>
    /// <returns>A new <see cref="ActionProvider"/> instance.</returns>
    public static ActionProvider DynamicInvoking<TResult>(Func<ValueTask<TResult>> function, string functionName)
    {
        function.CheckNotNull(nameof(function));
        functionName.CheckNotNull(nameof(functionName));

        return new(
            new DynamicObjectSource<Action>(() => () => function.Invoke().RunSync()),
            functionName);
    }

    /// <summary>
    /// Creates a new lazy <see cref="ActionProvider"/> from the invocation of the specified <paramref name="functionExpression"/>.
    /// </summary>
    /// <param name="functionExpression">The function expression.</param>
    /// <returns>A new <see cref="ActionProvider"/> instance.</returns>
    public static ActionProvider DynamicInvoking(Expression<Func<Task>> functionExpression)
    {
        functionExpression.CheckNotNull(nameof(functionExpression));

        var (function, functionName) = functionExpression.ExtractDelegateAndTextExpression();

        return DynamicInvoking(function, functionName);
    }

    /// <summary>
    /// Creates a new dynamic <see cref="ActionProvider"/> from the invocation of the specified <paramref name="function"/>
    /// with the specified <paramref name="functionName"/>.
    /// </summary>
    /// <param name="function">The function.</param>
    /// <param name="functionName">Name of the function.</param>
    /// <returns>A new <see cref="ActionProvider"/> instance.</returns>
    public static ActionProvider DynamicInvoking(Func<Task> function, string functionName)
    {
        function.CheckNotNull(nameof(function));
        functionName.CheckNotNull(nameof(functionName));

        return new(
            new DynamicObjectSource<Action>(() => function.Invoke().RunSync),
            functionName);
    }

    /// <summary>
    /// Creates a new lazy <see cref="ActionProvider"/> from the invocation of the specified <paramref name="functionExpression"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of the task result.</typeparam>
    /// <param name="functionExpression">The function expression.</param>
    /// <returns>A new <see cref="ActionProvider"/> instance.</returns>
    public static ActionProvider DynamicInvoking<TResult>(Expression<Func<Task<TResult>>> functionExpression)
    {
        functionExpression.CheckNotNull(nameof(functionExpression));

        var (function, functionName) = functionExpression.ExtractDelegateAndTextExpression();

        return DynamicInvoking(function, functionName);
    }

    /// <summary>
    /// Creates a new dynamic <see cref="ActionProvider"/> from the invocation of the specified <paramref name="function"/>
    /// with the specified <paramref name="functionName"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of the task result.</typeparam>
    /// <param name="function">The function.</param>
    /// <param name="functionName">Name of the function.</param>
    /// <returns>A new <see cref="ActionProvider"/> instance.</returns>
    public static ActionProvider DynamicInvoking<TResult>(Func<Task<TResult>> function, string functionName)
    {
        function.CheckNotNull(nameof(function));
        functionName.CheckNotNull(nameof(functionName));

        return new(
            new DynamicObjectSource<Action>(() => function.Invoke().RunSync),
            functionName);
    }

    internal static string BuildResultName(string functionName) =>
        functionName + ResultNameEnding;

    internal static string BuildExceptionName(string methodName)
    {
        string exceptionName = methodName.EndsWith(ResultNameEnding, StringComparison.Ordinal)
            ? methodName.Substring(0, methodName.Length - ResultNameEnding.Length)
            : methodName;

        return exceptionName + ExceptionNameEnding;
    }
}
