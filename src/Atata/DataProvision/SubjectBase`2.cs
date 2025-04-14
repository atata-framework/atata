namespace Atata;

/// <summary>
/// Represents the base test subject class.
/// </summary>
/// <typeparam name="TObject">The type of the object.</typeparam>
/// <typeparam name="TSubject">The type of the inherited subject class.</typeparam>
public abstract class SubjectBase<TObject, TSubject> : ObjectProvider<TObject, TSubject>
    where TSubject : SubjectBase<TObject, TSubject>
{
    private int _executedActionsCount;

    /// <summary>
    /// Initializes a new instance of the <see cref="SubjectBase{TObject, TSubject}"/> class.
    /// </summary>
    /// <param name="objectSource">The object source.</param>
    /// <param name="providerName">Name of the provider.</param>
    /// <param name="executionUnit">The execution unit, which can be <see langword="null"/>.</param>
    protected SubjectBase(
        IObjectSource<TObject> objectSource,
        string providerName,
        IAtataExecutionUnit? executionUnit = null)
        : base(objectSource, providerName, executionUnit)
    {
    }

    /// <inheritdoc/>
    protected override TSubject Owner => (TSubject)this;

    /// <inheritdoc cref="IObjectProvider{TObject}.Object"/>
    public new TObject Object => base.Object;

    /// <summary>
    /// Creates a new lazy result <see cref="Subject{TObject}"/> from the result of the specified <paramref name="functionExpression"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="functionExpression">The function expression.</param>
    /// <returns>A new <see cref="Subject{TObject}"/> instance.</returns>
    public Subject<TResult> ResultOf<TResult>(Expression<Func<TObject, TResult>> functionExpression)
    {
        Guard.ThrowIfNull(functionExpression);

        var (function, functionName) = functionExpression.ExtractDelegateAndTextExpression();

        return ResultOf(function, functionName);
    }

    /// <summary>
    /// Creates a new lazy result <see cref="Subject{TObject}"/> from the result of the specified <paramref name="function"/> with the specified <paramref name="functionName"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="function">The function.</param>
    /// <param name="functionName">Name of the function.</param>
    /// <returns>A new <see cref="Subject{TObject}"/> instance.</returns>
    public Subject<TResult> ResultOf<TResult>(Func<TObject, TResult> function, string functionName) =>
        SubjectOf(function, Subject.BuildResultName(functionName));

    /// <inheritdoc cref="ResultOf{TResult}(Expression{Func{TObject, TResult}})"/>
    public Subject<TResult> ResultOf<TResult>(Expression<Func<TObject, ValueTask<TResult>>> functionExpression)
    {
        Guard.ThrowIfNull(functionExpression);

        var (function, functionName) = functionExpression.ExtractDelegateAndTextExpression();

        return ResultOf(function, functionName);
    }

    /// <inheritdoc cref="ResultOf{TResult}(Func{TObject, TResult}, string)"/>
    public Subject<TResult> ResultOf<TResult>(Func<TObject, ValueTask<TResult>> function, string functionName) =>
        SubjectOf(function, Subject.BuildResultName(functionName));

    /// <inheritdoc cref="ResultOf{TResult}(Expression{Func{TObject, TResult}})"/>
    public Subject<TResult> ResultOf<TResult>(Expression<Func<TObject, Task<TResult>>> functionExpression)
    {
        Guard.ThrowIfNull(functionExpression);

        var (function, functionName) = functionExpression.ExtractDelegateAndTextExpression();

        return ResultOf(function, functionName);
    }

    /// <inheritdoc cref="ResultOf{TResult}(Func{TObject, TResult}, string)"/>
    public Subject<TResult> ResultOf<TResult>(Func<TObject, Task<TResult>> function, string functionName) =>
        SubjectOf(function, Subject.BuildResultName(functionName));

    /// <summary>
    /// Creates a new lazy <see cref="Subject{TObject}"/> from the result of the specified <paramref name="functionExpression"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="functionExpression">The function expression.</param>
    /// <returns>A new <see cref="Subject{TObject}"/> instance.</returns>
    public Subject<TResult> SubjectOf<TResult>(Expression<Func<TObject, TResult>> functionExpression)
    {
        Guard.ThrowIfNull(functionExpression);

        var (function, functionName) = functionExpression.ExtractDelegateAndTextExpression();

        return SubjectOf(function, functionName);
    }

    /// <summary>
    /// Creates a new lazy <see cref="Subject{TObject}"/> from the result of the specified <paramref name="function"/> with the specified <paramref name="functionName"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="function">The function.</param>
    /// <param name="functionName">Name of the function.</param>
    /// <returns>A new <see cref="Subject{TObject}"/> instance.</returns>
    public Subject<TResult> SubjectOf<TResult>(Func<TObject, TResult> function, string functionName)
    {
        Guard.ThrowIfNull(function);
        Guard.ThrowIfNull(functionName);

        return new(
            new LazyObjectSource<TResult, TObject>(this, function),
            functionName,
            ExecutionUnit);
    }

    /// <inheritdoc cref="SubjectOf{TResult}(Expression{Func{TObject, TResult}})"/>
    public Subject<TResult> SubjectOf<TResult>(Expression<Func<TObject, ValueTask<TResult>>> functionExpression)
    {
        Guard.ThrowIfNull(functionExpression);

        var (function, functionName) = functionExpression.ExtractDelegateAndTextExpression();

        return SubjectOf(function, functionName);
    }

    /// <inheritdoc cref="SubjectOf{TResult}(Func{TObject, TResult}, string)"/>
    public Subject<TResult> SubjectOf<TResult>(Func<TObject, ValueTask<TResult>> function, string functionName)
    {
        Guard.ThrowIfNull(function);
        Guard.ThrowIfNull(functionName);

        return new(
            new LazyObjectSource<TResult, TObject>(this, x => function(x).RunSync()),
            functionName,
            ExecutionUnit);
    }

    /// <inheritdoc cref="SubjectOf{TResult}(Expression{Func{TObject, TResult}})"/>
    public Subject<TResult> SubjectOf<TResult>(Expression<Func<TObject, Task<TResult>>> functionExpression)
    {
        Guard.ThrowIfNull(functionExpression);

        var (function, functionName) = functionExpression.ExtractDelegateAndTextExpression();

        return SubjectOf(function, functionName);
    }

    /// <inheritdoc cref="SubjectOf{TResult}(Func{TObject, TResult}, string)"/>
    public Subject<TResult> SubjectOf<TResult>(Func<TObject, Task<TResult>> function, string functionName)
    {
        Guard.ThrowIfNull(function);
        Guard.ThrowIfNull(functionName);

        return new(
            new LazyObjectSource<TResult, TObject>(this, x => function(x).RunSync()),
            functionName,
            ExecutionUnit);
    }

    /// <summary>
    /// Creates a new dynamic result <see cref="Subject{TObject}"/> from the result of the specified <paramref name="functionExpression"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="functionExpression">The function expression.</param>
    /// <returns>A new <see cref="Subject{TObject}"/> instance.</returns>
    public Subject<TResult> DynamicResultOf<TResult>(Expression<Func<TObject, TResult>> functionExpression)
    {
        Guard.ThrowIfNull(functionExpression);

        var (function, functionName) = functionExpression.ExtractDelegateAndTextExpression();

        return DynamicResultOf(function, functionName);
    }

    /// <summary>
    /// Creates a new dynamic result <see cref="Subject{TObject}"/> from the result of the specified <paramref name="function"/> with the specified <paramref name="functionName"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="function">The function.</param>
    /// <param name="functionName">Name of the function.</param>
    /// <returns>A new <see cref="Subject{TObject}"/> instance.</returns>
    public Subject<TResult> DynamicResultOf<TResult>(Func<TObject, TResult> function, string functionName) =>
        DynamicSubjectOf(function, Subject.BuildResultName(functionName));

    /// <inheritdoc cref="DynamicResultOf{TResult}(Expression{Func{TObject, TResult}})"/>
    public Subject<TResult> DynamicResultOf<TResult>(Expression<Func<TObject, ValueTask<TResult>>> functionExpression)
    {
        Guard.ThrowIfNull(functionExpression);

        var (function, functionName) = functionExpression.ExtractDelegateAndTextExpression();

        return DynamicResultOf(function, functionName);
    }

    /// <inheritdoc cref="DynamicResultOf{TResult}(Func{TObject, TResult}, string)"/>
    public Subject<TResult> DynamicResultOf<TResult>(Func<TObject, ValueTask<TResult>> function, string functionName) =>
        DynamicSubjectOf(function, Subject.BuildResultName(functionName));

    /// <inheritdoc cref="DynamicResultOf{TResult}(Expression{Func{TObject, TResult}})"/>
    public Subject<TResult> DynamicResultOf<TResult>(Expression<Func<TObject, Task<TResult>>> functionExpression)
    {
        Guard.ThrowIfNull(functionExpression);

        var (function, functionName) = functionExpression.ExtractDelegateAndTextExpression();

        return DynamicResultOf(function, functionName);
    }

    /// <inheritdoc cref="DynamicResultOf{TResult}(Func{TObject, TResult}, string)"/>
    public Subject<TResult> DynamicResultOf<TResult>(Func<TObject, Task<TResult>> function, string functionName) =>
        DynamicSubjectOf(function, Subject.BuildResultName(functionName));

    /// <summary>
    /// Creates a new dynamic <see cref="Subject{TObject}"/> from the result of the specified <paramref name="functionExpression"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="functionExpression">The function expression.</param>
    /// <returns>A new <see cref="Subject{TObject}"/> instance.</returns>
    public Subject<TResult> DynamicSubjectOf<TResult>(Expression<Func<TObject, TResult>> functionExpression)
    {
        Guard.ThrowIfNull(functionExpression);

        var (function, functionName) = functionExpression.ExtractDelegateAndTextExpression();

        return DynamicSubjectOf(function, functionName);
    }

    /// <summary>
    /// Creates a new dynamic <see cref="Subject{TObject}"/> from the result of the specified <paramref name="function"/> with the specified <paramref name="functionName"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="function">The function.</param>
    /// <param name="functionName">Name of the function.</param>
    /// <returns>A new <see cref="Subject{TObject}"/> instance.</returns>
    public Subject<TResult> DynamicSubjectOf<TResult>(Func<TObject, TResult> function, string functionName)
    {
        Guard.ThrowIfNull(function);
        Guard.ThrowIfNull(functionName);

        return new(
            new DynamicObjectSource<TResult, TObject>(this, function),
            functionName,
            ExecutionUnit);
    }

    /// <inheritdoc cref="DynamicSubjectOf{TResult}(Expression{Func{TObject, TResult}})"/>
    public Subject<TResult> DynamicSubjectOf<TResult>(Expression<Func<TObject, ValueTask<TResult>>> functionExpression)
    {
        Guard.ThrowIfNull(functionExpression);

        var (function, functionName) = functionExpression.ExtractDelegateAndTextExpression();

        return DynamicSubjectOf(function, functionName);
    }

    /// <inheritdoc cref="DynamicSubjectOf{TResult}(Func{TObject, TResult}, string)"/>
    public Subject<TResult> DynamicSubjectOf<TResult>(Func<TObject, ValueTask<TResult>> function, string functionName)
    {
        Guard.ThrowIfNull(function);
        Guard.ThrowIfNull(functionName);

        return new(
            new DynamicObjectSource<TResult, TObject>(this, x => function(x).RunSync()),
            functionName,
            ExecutionUnit);
    }

    /// <inheritdoc cref="DynamicSubjectOf{TResult}(Expression{Func{TObject, TResult}})"/>
    public Subject<TResult> DynamicSubjectOf<TResult>(Expression<Func<TObject, Task<TResult>>> functionExpression)
    {
        Guard.ThrowIfNull(functionExpression);

        var (function, functionName) = functionExpression.ExtractDelegateAndTextExpression();

        return DynamicSubjectOf(function, functionName);
    }

    /// <inheritdoc cref="DynamicSubjectOf{TResult}(Func{TObject, TResult}, string)"/>
    public Subject<TResult> DynamicSubjectOf<TResult>(Func<TObject, Task<TResult>> function, string functionName)
    {
        Guard.ThrowIfNull(function);
        Guard.ThrowIfNull(functionName);

        return new(
            new DynamicObjectSource<TResult, TObject>(this, x => function(x).RunSync()),
            functionName,
            ExecutionUnit);
    }

    /// <summary>
    /// Executes the specified <paramref name="actionExpression"/>.
    /// Appends the text representation of the <paramref name="actionExpression"/> to the <c>ProviderName</c> property of this instance.
    /// </summary>
    /// <param name="actionExpression">The action expression.</param>
    /// <returns>The same subject instance.</returns>
    public TSubject Act(Expression<Action<TObject>> actionExpression)
    {
        Guard.ThrowIfNull(actionExpression);

        var (action, actionName) = actionExpression.ExtractDelegateAndTextExpression();

        return Act(action, actionName);
    }

    /// <summary>
    /// Executes the specified <paramref name="action"/>.
    /// Appends the <paramref name="actionName"/> to the <c>ProviderName</c> property of this instance.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <param name="actionName">Name of the action.</param>
    /// <returns>The same subject instance.</returns>
    public TSubject Act(Action<TObject> action, string actionName)
    {
        Guard.ThrowIfNull(action);
        Guard.ThrowIfNull(actionName);

        action.Invoke(Object);

        AppendActActionToProviderName(actionName);

        return Owner;
    }

    /// <inheritdoc cref="Act(Expression{Action{TObject}})"/>
    public TSubject Act(Expression<Func<TObject, ValueTask>> actionExpression)
    {
        Guard.ThrowIfNull(actionExpression);

        var (action, actionName) = actionExpression.ExtractDelegateAndTextExpression();

        return Act(action, actionName);
    }

    /// <inheritdoc cref="Act(Action{TObject}, string)"/>
    public TSubject Act(Func<TObject, ValueTask> action, string actionName)
    {
        Guard.ThrowIfNull(action);
        Guard.ThrowIfNull(actionName);

        action.Invoke(Object).RunSync();

        AppendActActionToProviderName(actionName);

        return Owner;
    }

    /// <inheritdoc cref="Act(Expression{Action{TObject}})"/>
    public TSubject Act(Expression<Func<TObject, Task>> actionExpression)
    {
        Guard.ThrowIfNull(actionExpression);

        var (action, actionName) = actionExpression.ExtractDelegateAndTextExpression();

        return Act(action, actionName);
    }

    /// <inheritdoc cref="Act(Action{TObject}, string)"/>
    public TSubject Act(Func<TObject, Task> action, string actionName)
    {
        Guard.ThrowIfNull(action);
        Guard.ThrowIfNull(actionName);

        action.Invoke(Object).RunSync();

        AppendActActionToProviderName(actionName);

        return Owner;
    }

    /// <summary>
    /// Creates a new lazy <see cref="ActionProvider{TOwner}"/> from the invocation of the specified <paramref name="actionExpression"/>.
    /// </summary>
    /// <param name="actionExpression">The action expression.</param>
    /// <returns>A new <see cref="ActionProvider{TOwner}"/> instance.</returns>
    public ActionProvider<TSubject> Invoking(Expression<Action<TObject>> actionExpression)
    {
        Guard.ThrowIfNull(actionExpression);

        var (action, actionName) = actionExpression.ExtractDelegateAndTextExpression();

        return Invoking(action, actionName);
    }

    /// <summary>
    /// Creates a new lazy <see cref="ActionProvider{TOwner}"/> from the invocation of the specified <paramref name="action"/>
    /// with the specified <paramref name="actionName"/>.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <param name="actionName">Name of the action.</param>
    /// <returns>A new <see cref="ActionProvider{TOwner}"/> instance.</returns>
    public ActionProvider<TSubject> Invoking(Action<TObject> action, string actionName)
    {
        Guard.ThrowIfNull(action);
        Guard.ThrowIfNull(actionName);

        return new ActionProvider<TSubject>(
            (TSubject)this,
            new LazyObjectSource<Action, TObject>(this, x => () => action.Invoke(x)),
            actionName,
            ExecutionUnit);
    }

    /// <summary>
    /// Creates a new lazy <see cref="ActionProvider{TOwner}"/> from the invocation of the specified <paramref name="functionExpression"/>.
    /// </summary>
    /// <param name="functionExpression">The function expression.</param>
    /// <returns>A new <see cref="ActionProvider{TOwner}"/> instance.</returns>
    public ActionProvider<TSubject> Invoking(Expression<Func<TObject, ValueTask>> functionExpression)
    {
        Guard.ThrowIfNull(functionExpression);

        var (function, functionName) = functionExpression.ExtractDelegateAndTextExpression();

        return Invoking(function, functionName);
    }

    /// <summary>
    /// Creates a new lazy <see cref="ActionProvider{TOwner}"/> from the invocation of the specified <paramref name="function"/>
    /// with the specified <paramref name="functionName"/>.
    /// </summary>
    /// <param name="function">The function.</param>
    /// <param name="functionName">Name of the function.</param>
    /// <returns>A new <see cref="ActionProvider{TOwner}"/> instance.</returns>
    public ActionProvider<TSubject> Invoking(Func<TObject, ValueTask> function, string functionName)
    {
        Guard.ThrowIfNull(function);
        Guard.ThrowIfNull(functionName);

        return new ActionProvider<TSubject>(
            (TSubject)this,
            new LazyObjectSource<Action, TObject>(this, x => () => function.Invoke(x).RunSync()),
            functionName,
            ExecutionUnit);
    }

    /// <summary>
    /// Creates a new lazy <see cref="ActionProvider{TOwner}"/> from the invocation of the specified <paramref name="functionExpression"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of the value task result.</typeparam>
    /// <param name="functionExpression">The function expression.</param>
    /// <returns>A new <see cref="ActionProvider{TOwner}"/> instance.</returns>
    public ActionProvider<TSubject> Invoking<TResult>(Expression<Func<TObject, ValueTask<TResult>>> functionExpression)
    {
        Guard.ThrowIfNull(functionExpression);

        var (function, functionName) = functionExpression.ExtractDelegateAndTextExpression();

        return Invoking(function, functionName);
    }

    /// <summary>
    /// Creates a new lazy <see cref="ActionProvider{TOwner}"/> from the invocation of the specified <paramref name="function"/>
    /// with the specified <paramref name="functionName"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of the value task result.</typeparam>
    /// <param name="function">The function.</param>
    /// <param name="functionName">Name of the function.</param>
    /// <returns>A new <see cref="ActionProvider{TOwner}"/> instance.</returns>
    public ActionProvider<TSubject> Invoking<TResult>(Func<TObject, ValueTask<TResult>> function, string functionName)
    {
        Guard.ThrowIfNull(function);
        Guard.ThrowIfNull(functionName);

        return new ActionProvider<TSubject>(
            (TSubject)this,
            new LazyObjectSource<Action, TObject>(this, x => () => function.Invoke(x).RunSync()),
            functionName,
            ExecutionUnit);
    }

    /// <summary>
    /// Creates a new lazy <see cref="ActionProvider{TOwner}"/> from the invocation of the specified <paramref name="functionExpression"/>.
    /// </summary>
    /// <param name="functionExpression">The function expression.</param>
    /// <returns>A new <see cref="ActionProvider{TOwner}"/> instance.</returns>
    public ActionProvider<TSubject> Invoking(Expression<Func<TObject, Task>> functionExpression)
    {
        Guard.ThrowIfNull(functionExpression);

        var (function, functionName) = functionExpression.ExtractDelegateAndTextExpression();

        return Invoking(function, functionName);
    }

    /// <summary>
    /// Creates a new lazy <see cref="ActionProvider{TOwner}"/> from the invocation of the specified <paramref name="function"/>
    /// with the specified <paramref name="functionName"/>.
    /// </summary>
    /// <param name="function">The function.</param>
    /// <param name="functionName">Name of the function.</param>
    /// <returns>A new <see cref="ActionProvider{TOwner}"/> instance.</returns>
    public ActionProvider<TSubject> Invoking(Func<TObject, Task> function, string functionName)
    {
        Guard.ThrowIfNull(function);
        Guard.ThrowIfNull(functionName);

        return new ActionProvider<TSubject>(
            (TSubject)this,
            new LazyObjectSource<Action, TObject>(this, x => () => function.Invoke(x).RunSync()),
            functionName,
            ExecutionUnit);
    }

    /// <summary>
    /// Creates a new lazy <see cref="ActionProvider{TOwner}"/> from the invocation of the specified <paramref name="functionExpression"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of the task result.</typeparam>
    /// <param name="functionExpression">The function expression.</param>
    /// <returns>A new <see cref="ActionProvider{TOwner}"/> instance.</returns>
    public ActionProvider<TSubject> Invoking<TResult>(Expression<Func<TObject, Task<TResult>>> functionExpression)
    {
        Guard.ThrowIfNull(functionExpression);

        var (function, functionName) = functionExpression.ExtractDelegateAndTextExpression();

        return Invoking(function, functionName);
    }

    /// <summary>
    /// Creates a new lazy <see cref="ActionProvider{TOwner}"/> from the invocation of the specified <paramref name="function"/>
    /// with the specified <paramref name="functionName"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of the task result.</typeparam>
    /// <param name="function">The function.</param>
    /// <param name="functionName">Name of the function.</param>
    /// <returns>A new <see cref="ActionProvider{TOwner}"/> instance.</returns>
    public ActionProvider<TSubject> Invoking<TResult>(Func<TObject, Task<TResult>> function, string functionName)
    {
        Guard.ThrowIfNull(function);
        Guard.ThrowIfNull(functionName);

        return new ActionProvider<TSubject>(
            (TSubject)this,
            new LazyObjectSource<Action, TObject>(this, x => () => function.Invoke(x).RunSync()),
            functionName,
            ExecutionUnit);
    }

    /// <summary>
    /// Creates a new dynamic <see cref="ActionProvider{TOwner}"/> from the invocation of the specified <paramref name="actionExpression"/>.
    /// </summary>
    /// <param name="actionExpression">The action expression.</param>
    /// <returns>A new <see cref="ActionProvider{TOwner}"/> instance.</returns>
    public ActionProvider<TSubject> DynamicInvoking(Expression<Action<TObject>> actionExpression)
    {
        Guard.ThrowIfNull(actionExpression);

        var (action, actionName) = actionExpression.ExtractDelegateAndTextExpression();

        return DynamicInvoking(action, actionName);
    }

    /// <summary>
    /// Creates a new dynamic <see cref="ActionProvider{TOwner}"/> from the invocation of the specified <paramref name="action"/>
    /// with the specified <paramref name="actionName"/>.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <param name="actionName">Name of the action.</param>
    /// <returns>A new <see cref="ActionProvider{TOwner}"/> instance.</returns>
    public ActionProvider<TSubject> DynamicInvoking(Action<TObject> action, string actionName)
    {
        Guard.ThrowIfNull(action);
        Guard.ThrowIfNull(actionName);

        return new ActionProvider<TSubject>(
            (TSubject)this,
            new DynamicObjectSource<Action, TObject>(this, x => () => action.Invoke(x)),
            actionName,
            ExecutionUnit);
    }

    /// <summary>
    /// Creates a new dynamic <see cref="ActionProvider{TOwner}"/> from the invocation of the specified <paramref name="functionExpression"/>.
    /// </summary>
    /// <param name="functionExpression">The function expression.</param>
    /// <returns>A new <see cref="ActionProvider{TOwner}"/> instance.</returns>
    public ActionProvider<TSubject> DynamicInvoking(Expression<Func<TObject, ValueTask>> functionExpression)
    {
        Guard.ThrowIfNull(functionExpression);

        var (function, functionName) = functionExpression.ExtractDelegateAndTextExpression();

        return DynamicInvoking(function, functionName);
    }

    /// <summary>
    /// Creates a new dynamic <see cref="ActionProvider{TOwner}"/> from the invocation of the specified <paramref name="function"/>
    /// with the specified <paramref name="functionName"/>.
    /// </summary>
    /// <param name="function">The function.</param>
    /// <param name="functionName">Name of the function.</param>
    /// <returns>A new <see cref="ActionProvider{TOwner}"/> instance.</returns>
    public ActionProvider<TSubject> DynamicInvoking(Func<TObject, ValueTask> function, string functionName)
    {
        Guard.ThrowIfNull(function);
        Guard.ThrowIfNull(functionName);

        return new ActionProvider<TSubject>(
            (TSubject)this,
            new DynamicObjectSource<Action, TObject>(this, x => () => function.Invoke(x).RunSync()),
            functionName,
            ExecutionUnit);
    }

    /// <summary>
    /// Creates a new dynamic <see cref="ActionProvider{TOwner}"/> from the invocation of the specified <paramref name="functionExpression"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of the value task result.</typeparam>
    /// <param name="functionExpression">The function expression.</param>
    /// <returns>A new <see cref="ActionProvider{TOwner}"/> instance.</returns>
    public ActionProvider<TSubject> DynamicInvoking<TResult>(Expression<Func<TObject, ValueTask<TResult>>> functionExpression)
    {
        Guard.ThrowIfNull(functionExpression);

        var (function, functionName) = functionExpression.ExtractDelegateAndTextExpression();

        return DynamicInvoking(function, functionName);
    }

    /// <summary>
    /// Creates a new dynamic <see cref="ActionProvider{TOwner}"/> from the invocation of the specified <paramref name="function"/>
    /// with the specified <paramref name="functionName"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of the value task result.</typeparam>
    /// <param name="function">The function.</param>
    /// <param name="functionName">Name of the function.</param>
    /// <returns>A new <see cref="ActionProvider{TOwner}"/> instance.</returns>
    public ActionProvider<TSubject> DynamicInvoking<TResult>(Func<TObject, ValueTask<TResult>> function, string functionName)
    {
        Guard.ThrowIfNull(function);
        Guard.ThrowIfNull(functionName);

        return new ActionProvider<TSubject>(
            (TSubject)this,
            new DynamicObjectSource<Action, TObject>(this, x => () => function.Invoke(x).RunSync()),
            functionName,
            ExecutionUnit);
    }

    /// <summary>
    /// Creates a new dynamic <see cref="ActionProvider{TOwner}"/> from the invocation of the specified <paramref name="functionExpression"/>.
    /// </summary>
    /// <param name="functionExpression">The function expression.</param>
    /// <returns>A new <see cref="ActionProvider{TOwner}"/> instance.</returns>
    public ActionProvider<TSubject> DynamicInvoking(Expression<Func<TObject, Task>> functionExpression)
    {
        Guard.ThrowIfNull(functionExpression);

        var (function, functionName) = functionExpression.ExtractDelegateAndTextExpression();

        return DynamicInvoking(function, functionName);
    }

    /// <summary>
    /// Creates a new dynamic <see cref="ActionProvider{TOwner}"/> from the invocation of the specified <paramref name="function"/>
    /// with the specified <paramref name="functionName"/>.
    /// </summary>
    /// <param name="function">The function.</param>
    /// <param name="functionName">Name of the function.</param>
    /// <returns>A new <see cref="ActionProvider{TOwner}"/> instance.</returns>
    public ActionProvider<TSubject> DynamicInvoking(Func<TObject, Task> function, string functionName)
    {
        Guard.ThrowIfNull(function);
        Guard.ThrowIfNull(functionName);

        return new ActionProvider<TSubject>(
            (TSubject)this,
            new DynamicObjectSource<Action, TObject>(this, x => () => function.Invoke(x).RunSync()),
            functionName,
            ExecutionUnit);
    }

    /// <summary>
    /// Creates a new dynamic <see cref="ActionProvider{TOwner}"/> from the invocation of the specified <paramref name="functionExpression"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of the task result.</typeparam>
    /// <param name="functionExpression">The function expression.</param>
    /// <returns>A new <see cref="ActionProvider{TOwner}"/> instance.</returns>
    public ActionProvider<TSubject> DynamicInvoking<TResult>(Expression<Func<TObject, Task<TResult>>> functionExpression)
    {
        Guard.ThrowIfNull(functionExpression);

        var (function, functionName) = functionExpression.ExtractDelegateAndTextExpression();

        return DynamicInvoking(function, functionName);
    }

    /// <summary>
    /// Creates a new dynamic <see cref="ActionProvider{TOwner}"/> from the invocation of the specified <paramref name="function"/>
    /// with the specified <paramref name="functionName"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of the task result.</typeparam>
    /// <param name="function">The function.</param>
    /// <param name="functionName">Name of the function.</param>
    /// <returns>A new <see cref="ActionProvider{TOwner}"/> instance.</returns>
    public ActionProvider<TSubject> DynamicInvoking<TResult>(Func<TObject, Task<TResult>> function, string functionName)
    {
        Guard.ThrowIfNull(function);
        Guard.ThrowIfNull(functionName);

        return new ActionProvider<TSubject>(
            (TSubject)this,
            new DynamicObjectSource<Action, TObject>(this, x => () => function.Invoke(x).RunSync()),
            functionName,
            ExecutionUnit);
    }

    /// <summary>
    /// Executes aggregate assertion for the current subject using <see cref="AtataContext.AggregateAssert(Action, string)" /> method.
    /// </summary>
    /// <param name="action">The action to execute in scope of aggregate assertion.</param>
    /// <param name="assertionScopeName">
    /// Name of the scope being asserted.
    /// Is used to identify the assertion section in log.
    /// If it is <see langword="null"/>, <see cref="ObjectProvider{TObject, TOwner}.ProviderName"/> is used instead.
    /// </param>
    /// <returns>The instance of this page object.</returns>
    public TSubject AggregateAssert(Action<TSubject> action, string? assertionScopeName = null)
    {
        Guard.ThrowIfNull(action);

        assertionScopeName ??= ProviderName;

        ResolveAtataContext()
            .AggregateAssert(() => action((TSubject)this), assertionScopeName);

        return Owner;
    }

    private void AppendActActionToProviderName(string actionName)
    {
        ProviderName = _executedActionsCount == 0
            ? $"{ProviderName}{{ {actionName} }}"
            : $"{ProviderName[..^2]}; {actionName} }}";

        _executedActionsCount++;
    }
}
