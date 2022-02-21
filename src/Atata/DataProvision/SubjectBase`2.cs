using System;
using System.Linq.Expressions;

namespace Atata
{
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
        protected SubjectBase(IObjectSource<TObject> objectSource, string providerName)
            : base(objectSource, providerName)
        {
        }

        /// <inheritdoc/>
        protected override TSubject Owner => (TSubject)this;

        /// <inheritdoc cref="IObjectProvider{TObject}.Object"/>
        public new TObject Object => base.Object;

        /// <summary>
        /// Creates a new lazy result <see cref="Subject{T}"/> from the result of the specified <paramref name="functionExpression"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="functionExpression">The function expression.</param>
        /// <returns>A new <see cref="Subject{T}"/> instance.</returns>
        public Subject<TResult> ResultOf<TResult>(Expression<Func<TObject, TResult>> functionExpression)
        {
            functionExpression.CheckNotNull(nameof(functionExpression));

            var function = functionExpression.Compile();
            string functionName = ObjectExpressionStringBuilder.ExpressionToString(functionExpression);

            return ResultOf(function, functionName);
        }

        /// <summary>
        /// Creates a new lazy result <see cref="Subject{T}"/> from the result of the specified <paramref name="function"/> with the specified <paramref name="functionName"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="function">The function.</param>
        /// <param name="functionName">Name of the function.</param>
        /// <returns>A new <see cref="Subject{T}"/> instance.</returns>
        public Subject<TResult> ResultOf<TResult>(Func<TObject, TResult> function, string functionName) =>
            SubjectOf(function, Subject.BuildResultName(functionName));

        /// <summary>
        /// Creates a new lazy <see cref="Subject{T}"/> from the result of the specified <paramref name="functionExpression"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="functionExpression">The function expression.</param>
        /// <returns>A new <see cref="Subject{T}"/> instance.</returns>
        public Subject<TResult> SubjectOf<TResult>(Expression<Func<TObject, TResult>> functionExpression)
        {
            functionExpression.CheckNotNull(nameof(functionExpression));

            var function = functionExpression.Compile();
            string functionName = ObjectExpressionStringBuilder.ExpressionToString(functionExpression);

            return SubjectOf(function, functionName);
        }

        /// <summary>
        /// Creates a new lazy <see cref="Subject{T}"/> from the result of the specified <paramref name="function"/> with the specified <paramref name="functionName"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="function">The function.</param>
        /// <param name="functionName">Name of the function.</param>
        /// <returns>A new <see cref="Subject{T}"/> instance.</returns>
        public Subject<TResult> SubjectOf<TResult>(Func<TObject, TResult> function, string functionName)
        {
            function.CheckNotNull(nameof(function));
            functionName.CheckNotNull(nameof(functionName));

            return new Subject<TResult>(
                new LazyObjectSource<TResult, TObject>(this, function),
                functionName);
        }

        /// <summary>
        /// Creates a new dynamic result <see cref="Subject{T}"/> from the result of the specified <paramref name="functionExpression"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="functionExpression">The function expression.</param>
        /// <returns>A new <see cref="Subject{T}"/> instance.</returns>
        public Subject<TResult> DynamicResultOf<TResult>(Expression<Func<TObject, TResult>> functionExpression)
        {
            functionExpression.CheckNotNull(nameof(functionExpression));

            var function = functionExpression.Compile();
            string functionName = ObjectExpressionStringBuilder.ExpressionToString(functionExpression);

            return DynamicResultOf(function, functionName);
        }

        /// <summary>
        /// Creates a new dynamic result <see cref="Subject{T}"/> from the result of the specified <paramref name="function"/> with the specified <paramref name="functionName"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="function">The function.</param>
        /// <param name="functionName">Name of the function.</param>
        /// <returns>A new <see cref="Subject{T}"/> instance.</returns>
        public Subject<TResult> DynamicResultOf<TResult>(Func<TObject, TResult> function, string functionName) =>
            DynamicSubjectOf(function, Subject.BuildResultName(functionName));

        /// <summary>
        /// Creates a new dynamic <see cref="Subject{T}"/> from the result of the specified <paramref name="functionExpression"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="functionExpression">The function expression.</param>
        /// <returns>A new <see cref="Subject{T}"/> instance.</returns>
        public Subject<TResult> DynamicSubjectOf<TResult>(Expression<Func<TObject, TResult>> functionExpression)
        {
            functionExpression.CheckNotNull(nameof(functionExpression));

            var function = functionExpression.Compile();
            string functionName = ObjectExpressionStringBuilder.ExpressionToString(functionExpression);

            return DynamicSubjectOf(function, functionName);
        }

        /// <summary>
        /// Creates a new dynamic <see cref="Subject{T}"/> from the result of the specified <paramref name="function"/> with the specified <paramref name="functionName"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="function">The function.</param>
        /// <param name="functionName">Name of the function.</param>
        /// <returns>A new <see cref="Subject{T}"/> instance.</returns>
        public Subject<TResult> DynamicSubjectOf<TResult>(Func<TObject, TResult> function, string functionName)
        {
            function.CheckNotNull(nameof(function));
            functionName.CheckNotNull(nameof(functionName));

            return new Subject<TResult>(
                new DynamicObjectSource<TResult, TObject>(this, function),
                functionName);
        }

        /// <summary>
        /// Executes the specified <paramref name="actionExpression"/>.
        /// Appends the text representation of the <paramref name="actionExpression"/> to the <c>ProviderName</c> property of this instance.
        /// </summary>
        /// <param name="actionExpression">The action expression.</param>
        /// <returns>The same subject instance.</returns>
        public TSubject Act(Expression<Action<TObject>> actionExpression)
        {
            actionExpression.CheckNotNull(nameof(actionExpression));

            var action = actionExpression.Compile();
            string actionName = ObjectExpressionStringBuilder.ExpressionToString(actionExpression);

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
            action.CheckNotNull(nameof(action));
            actionName.CheckNotNull(nameof(actionName));

            action.Invoke(Object);

            ProviderName = _executedActionsCount == 0
                ? $"{ProviderName}{{ {actionName} }}"
                : $"{ProviderName.Substring(0, ProviderName.Length - 2)}; {actionName} }}";

            _executedActionsCount++;

            return Owner;
        }

        /// <summary>
        /// Creates a new lazy <see cref="ActionProvider{TOwner}"/> from the invocation of the specified <paramref name="actionExpression"/>.
        /// </summary>
        /// <param name="actionExpression">The action expression.</param>
        /// <returns>A new <see cref="ActionProvider{TOwner}"/> instance.</returns>
        public ActionProvider<TSubject> Invoking(Expression<Action<TObject>> actionExpression)
        {
            actionExpression.CheckNotNull(nameof(actionExpression));

            var action = actionExpression.Compile();
            string actionName = ObjectExpressionStringBuilder.ExpressionToString(actionExpression);

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
            action.CheckNotNull(nameof(action));
            actionName.CheckNotNull(nameof(actionName));

            return new ActionProvider<TSubject>(
                (TSubject)this,
                new LazyObjectSource<Action, TObject>(this, x => () => action.Invoke(x)),
                actionName);
        }

        /// <summary>
        /// Creates a new lazy <see cref="ActionProvider{TOwner}"/> from the invocation of the specified <paramref name="actionExpression"/>.
        /// </summary>
        /// <param name="actionExpression">The action expression.</param>
        /// <returns>A new <see cref="ActionProvider{TOwner}"/> instance.</returns>
        public ActionProvider<TSubject> DynamicInvoking(Expression<Action<TObject>> actionExpression)
        {
            actionExpression.CheckNotNull(nameof(actionExpression));

            var action = actionExpression.Compile();
            string actionName = ObjectExpressionStringBuilder.ExpressionToString(actionExpression);

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
            action.CheckNotNull(nameof(action));
            actionName.CheckNotNull(nameof(actionName));

            return new ActionProvider<TSubject>(
                (TSubject)this,
                new DynamicObjectSource<Action, TObject>(this, x => () => action.Invoke(x)),
                actionName);
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
        public TSubject AggregateAssert(Action<TSubject> action, string assertionScopeName = null)
        {
            action.CheckNotNull(nameof(action));

            assertionScopeName = assertionScopeName ?? ProviderName;

            ResolveAtataContext()
                .AggregateAssert(() => action((TSubject)this), assertionScopeName);

            return Owner;
        }
    }
}
