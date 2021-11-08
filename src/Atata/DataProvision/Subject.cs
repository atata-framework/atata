using System;
using System.Linq.Expressions;

namespace Atata
{
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

            var function = functionExpression.Compile();
            string functionName = ObjectExpressionStringBuilder.ExpressionToString(functionExpression);

            return ResultOf(function, functionName);
        }

        /// <inheritdoc cref="SubjectBase{TObject, TSubject}.ResultOf{TResult}(Func{TObject, TResult}, string)"/>
        public static Subject<TResult> ResultOf<TResult>(Func<TResult> function, string functionName) =>
            SubjectOf(function, BuildResultName(functionName));

        /// <inheritdoc cref="SubjectBase{TObject, TSubject}.SubjectOf{TResult}(Expression{Func{TObject, TResult}})"/>
        public static Subject<TResult> SubjectOf<TResult>(Expression<Func<TResult>> functionExpression)
        {
            functionExpression.CheckNotNull(nameof(functionExpression));

            var function = functionExpression.Compile();
            string functionName = ObjectExpressionStringBuilder.ExpressionToString(functionExpression);

            return SubjectOf(function, functionName);
        }

        /// <inheritdoc cref="SubjectBase{TObject, TSubject}.SubjectOf{TResult}(Func{TObject, TResult}, string)"/>
        public static Subject<TResult> SubjectOf<TResult>(Func<TResult> function, string functionName)
        {
            function.CheckNotNull(nameof(function));
            functionName.CheckNotNull(nameof(functionName));

            return new Subject<TResult>(
                new LazyObjectSource<TResult>(function),
                functionName);
        }

        /// <inheritdoc cref="SubjectBase{TObject, TSubject}.DynamicResultOf{TResult}(Expression{Func{TObject, TResult}})"/>
        public static Subject<TResult> DynamicResultOf<TResult>(Expression<Func<TResult>> functionExpression)
        {
            functionExpression.CheckNotNull(nameof(functionExpression));

            var function = functionExpression.Compile();
            string functionName = ObjectExpressionStringBuilder.ExpressionToString(functionExpression);

            return DynamicResultOf(function, functionName);
        }

        /// <inheritdoc cref="SubjectBase{TObject, TSubject}.DynamicResultOf{TResult}(Func{TObject, TResult}, string)"/>
        public static Subject<TResult> DynamicResultOf<TResult>(Func<TResult> function, string functionName) =>
            DynamicSubjectOf(function, BuildResultName(functionName));

        /// <inheritdoc cref="SubjectBase{TObject, TSubject}.DynamicSubjectOf{TResult}(Expression{Func{TObject, TResult}})"/>
        public static Subject<TResult> DynamicSubjectOf<TResult>(Expression<Func<TResult>> functionExpression)
        {
            functionExpression.CheckNotNull(nameof(functionExpression));

            var function = functionExpression.Compile();
            string functionName = ObjectExpressionStringBuilder.ExpressionToString(functionExpression);

            return DynamicSubjectOf(function, functionName);
        }

        /// <inheritdoc cref="SubjectBase{TObject, TSubject}.DynamicSubjectOf{TResult}(Func{TObject, TResult}, string)"/>
        public static Subject<TResult> DynamicSubjectOf<TResult>(Func<TResult> function, string functionName)
        {
            function.CheckNotNull(nameof(function));
            functionName.CheckNotNull(nameof(functionName));

            return new Subject<TResult>(
                new DynamicObjectSource<TResult>(function),
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

            var action = actionExpression.Compile();
            string actionName = ObjectExpressionStringBuilder.ExpressionToString(actionExpression);

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

            return new ActionProvider(
                new LazyObjectSource<Action>(() => action),
                actionName);
        }

        /// <summary>
        /// Creates a new lazy <see cref="ActionProvider"/> from the invocation of the specified <paramref name="actionExpression"/>.
        /// </summary>
        /// <param name="actionExpression">The action expression.</param>
        /// <returns>A new <see cref="ActionProvider"/> instance.</returns>
        public static ActionProvider DynamicInvoking(Expression<Action> actionExpression)
        {
            actionExpression.CheckNotNull(nameof(actionExpression));

            var action = actionExpression.Compile();
            string actionName = ObjectExpressionStringBuilder.ExpressionToString(actionExpression);

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

            return new ActionProvider(
                new DynamicObjectSource<Action>(() => action),
                actionName);
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
}
