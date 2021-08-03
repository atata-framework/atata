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

        /// <summary>
        /// Creates a new lazy <see cref="Subject{TResult}"/> from the result of the specified <paramref name="functionExpression"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="functionExpression">The function expression.</param>
        /// <returns>A new <see cref="Subject{TResult}"/> instance.</returns>
        public Subject<TResult> ResultOf<TResult>(Expression<Func<TObject, TResult>> functionExpression)
        {
            functionExpression.CheckNotNull(nameof(functionExpression));

            var function = functionExpression.Compile();
            string functionName = ObjectExpressionStringBuilder.ExpressionToString(functionExpression);

            return ResultOf(function, functionName);
        }

        /// <summary>
        /// Creates a new lazy <see cref="Subject{TResult}"/> from the result of the specified <paramref name="function"/> with the specified <paramref name="functionName"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="function">The function.</param>
        /// <param name="functionName">Name of the function.</param>
        /// <returns>A new <see cref="Subject{TResult}"/> instance.</returns>
        public Subject<TResult> ResultOf<TResult>(Func<TObject, TResult> function, string functionName)
        {
            function.CheckNotNull(nameof(function));
            functionName.CheckNotNull(nameof(functionName));

            return new Subject<TResult>(
                new LazyObjectSource<TResult, TObject>(this, function),
                Subject.BuildResultName(functionName));
        }

        /// <summary>
        /// Creates a new dynamic <see cref="Subject{TResult}"/> from the result of the specified <paramref name="functionExpression"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="functionExpression">The function expression.</param>
        /// <returns>A new <see cref="Subject{TResult}"/> instance.</returns>
        public Subject<TResult> DynamicResultOf<TResult>(Expression<Func<TObject, TResult>> functionExpression)
        {
            functionExpression.CheckNotNull(nameof(functionExpression));

            var function = functionExpression.Compile();
            string functionName = ObjectExpressionStringBuilder.ExpressionToString(functionExpression);

            return DynamicResultOf(function, functionName);
        }

        /// <summary>
        /// Creates a new dynamic <see cref="Subject{TResult}"/> from the result of the specified <paramref name="function"/> with the specified <paramref name="functionName"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="function">The function.</param>
        /// <param name="functionName">Name of the function.</param>
        /// <returns>A new <see cref="Subject{TResult}"/> instance.</returns>
        public Subject<TResult> DynamicResultOf<TResult>(Func<TObject, TResult> function, string functionName)
        {
            function.CheckNotNull(nameof(function));
            functionName.CheckNotNull(nameof(functionName));

            return new Subject<TResult>(
                new DynamicObjectSource<TResult, TObject>(this, function),
                Subject.BuildResultName(functionName));
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

            action.Invoke(Value);

            ProviderName = _executedActionsCount == 0
                ? $"{ProviderName}{{ {actionName} }}"
                : $"{ProviderName.Substring(0, ProviderName.Length - 2)}; {actionName} }}";

            _executedActionsCount++;

            return Owner;
        }
    }
}
