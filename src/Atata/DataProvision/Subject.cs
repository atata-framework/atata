using System;
using System.Linq.Expressions;

namespace Atata
{
    /// <summary>
    /// Provides a set of static methods for testing static class methods and properties.
    /// </summary>
    public static class Subject
    {
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
        public static Subject<TResult> ResultOf<TResult>(Func<TResult> function, string functionName)
        {
            function.CheckNotNull(nameof(function));
            functionName.CheckNotNull(nameof(functionName));

            return new Subject<TResult>(
                new LazyObjectSource<TResult>(function),
                BuildResultName(functionName));
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
        public static Subject<TResult> DynamicResultOf<TResult>(Func<TResult> function, string functionName)
        {
            function.CheckNotNull(nameof(function));
            functionName.CheckNotNull(nameof(functionName));

            return new Subject<TResult>(
                new DynamicObjectSource<TResult>(function),
                BuildResultName(functionName));
        }

        internal static string BuildResultName(string functionName) =>
            $"{functionName} => result";
    }
}
