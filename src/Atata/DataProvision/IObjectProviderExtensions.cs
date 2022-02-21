using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Atata
{
    /// <summary>
    /// Provides a set of extension methods for <see cref="IObjectProvider{TObject, TOwner}"/>.
    /// </summary>
    public static class IObjectProviderExtensions
    {
        /// <summary>
        /// Creates a provider of value resolved from <paramref name="valueExpression"/> argument.
        /// The created provider can be either dynamic or lazy,
        /// according to <see cref="IObjectProvider{TObject, TOwner}.IsDynamic"/> property of <paramref name="source"/> object provider.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="valueExpression">The value expression.</param>
        /// <returns>A <see cref="ValueProvider{TValue, TOwner}"/> instance.</returns>
        public static ValueProvider<TResult, TOwner> ValueOf<TSource, TResult, TOwner>(
            this IObjectProvider<TSource, TOwner> source,
            Expression<Func<TSource, TResult>> valueExpression)
            =>
            source.CheckNotNull(nameof(source)).IsDynamic
                ? source.DynamicValueOf(valueExpression)
                : source.LazyValueOf(valueExpression);

        /// <summary>
        /// Creates a provider of value that is taken from <paramref name="valueGetFunction"/> with <paramref name="valueName"/> as a provider name.
        /// The created provider can be either dynamic or lazy,
        /// according to <see cref="IObjectProvider{TObject, TOwner}.IsDynamic"/> property of <paramref name="source"/> object provider.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="valueGetFunction">The value get function.</param>
        /// <param name="valueName">Name of the value.</param>
        /// <returns>A <see cref="ValueProvider{TValue, TOwner}"/> instance.</returns>
        public static ValueProvider<TResult, TOwner> ValueOf<TSource, TResult, TOwner>(
            this IObjectProvider<TSource, TOwner> source,
            Func<TSource, TResult> valueGetFunction,
            string valueName)
            =>
            source.CheckNotNull(nameof(source)).IsDynamic
                ? source.DynamicValueOf(valueGetFunction, valueName)
                : source.LazyValueOf(valueGetFunction, valueName);

        /// <summary>
        /// Creates a dynamic provider of value resolved from <paramref name="valueExpression"/> argument.
        /// Dynamic provider on each value request can return different value.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="valueExpression">The value expression.</param>
        /// <returns>A <see cref="ValueProvider{TValue, TOwner}"/> instance.</returns>
        public static ValueProvider<TResult, TOwner> DynamicValueOf<TSource, TResult, TOwner>(
            this IObjectProvider<TSource, TOwner> source,
            Expression<Func<TSource, TResult>> valueExpression)
        {
            source.CheckNotNull(nameof(source));
            valueExpression.CheckNotNull(nameof(valueExpression));

            string valueName = ConvertToValueName(valueExpression);
            var valueFunction = valueExpression.Compile();

            return source.DynamicValueOf(valueFunction, valueName);
        }

        /// <summary>
        /// Creates a dynamic provider of value that is taken from <paramref name="valueGetFunction"/> with <paramref name="valueName"/> as a provider name.
        /// Dynamic provider on each value request can return different value.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="valueGetFunction">The value get function.</param>
        /// <param name="valueName">Name of the value.</param>
        /// <returns>A <see cref="ValueProvider{TValue, TOwner}"/> instance.</returns>
        public static ValueProvider<TResult, TOwner> DynamicValueOf<TSource, TResult, TOwner>(
            this IObjectProvider<TSource, TOwner> source,
            Func<TSource, TResult> valueGetFunction,
            string valueName)
        {
            source.CheckNotNull(nameof(source));
            valueGetFunction.CheckNotNull(nameof(valueGetFunction));
            valueName.CheckNotNull(nameof(valueName));

            return new ValueProvider<TResult, TOwner>(
                source.Owner,
                new DynamicObjectSource<TResult, TSource>(source, valueGetFunction),
                valueName);
        }

        /// <summary>
        /// Creates a lazy provider of value resolved from <paramref name="valueExpression"/> argument.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="valueExpression">The value expression.</param>
        /// <returns>A <see cref="ValueProvider{TValue, TOwner}"/> instance.</returns>
        public static ValueProvider<TResult, TOwner> LazyValueOf<TSource, TResult, TOwner>(
            this IObjectProvider<TSource, TOwner> source,
            Expression<Func<TSource, TResult>> valueExpression)
        {
            source.CheckNotNull(nameof(source));
            valueExpression.CheckNotNull(nameof(valueExpression));

            string valueName = ConvertToValueName(valueExpression);
            var valueFunction = valueExpression.Compile();

            return source.LazyValueOf(valueFunction, valueName);
        }

        /// <summary>
        /// Creates a lazy provider of value that is taken from <paramref name="valueGetFunction"/> with <paramref name="valueName"/> as a provider name.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="valueGetFunction">The value get function.</param>
        /// <param name="valueName">Name of the value.</param>
        /// <returns>A <see cref="ValueProvider{TValue, TOwner}"/> instance.</returns>
        public static ValueProvider<TResult, TOwner> LazyValueOf<TSource, TResult, TOwner>(
            this IObjectProvider<TSource, TOwner> source,
            Func<TSource, TResult> valueGetFunction,
            string valueName)
        {
            source.CheckNotNull(nameof(source));
            valueGetFunction.CheckNotNull(nameof(valueGetFunction));
            valueName.CheckNotNull(nameof(valueName));

            return new ValueProvider<TResult, TOwner>(
                source.Owner,
                new LazyObjectSource<TResult, TSource>(source, valueGetFunction),
                valueName);
        }

        /// <summary>
        /// Creates a enumerable provider of value that is taken from <paramref name="valueGetFunction"/> with <paramref name="valueName"/> as a provider name.
        /// The created provider can be either dynamic or lazy,
        /// according to <see cref="IObjectProvider{TObject, TOwner}.IsDynamic"/> property of <paramref name="source"/> object provider.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="valueGetFunction">The value get function.</param>
        /// <param name="valueName">Name of the value.</param>
        /// <returns>An <see cref="EnumerableValueProvider{TItem, TOwner}"/> instance.</returns>
        public static EnumerableValueProvider<TResult, TOwner> EnumerableValueOf<TSource, TResult, TOwner>(
            this IObjectProvider<TSource, TOwner> source,
            Func<TSource, IEnumerable<TResult>> valueGetFunction,
            string valueName)
            =>
            source.CheckNotNull(nameof(source)).IsDynamic
                ? source.DynamicEnumerableValueOf(valueGetFunction, valueName)
                : source.LazyEnumerableValueOf(valueGetFunction, valueName);

        /// <summary>
        /// Creates a dynamic enumerable provider of value that is taken from <paramref name="valueGetFunction"/> with <paramref name="valueName"/> as a provider name.
        /// Dynamic provider on each value request can return different value.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="valueGetFunction">The value get function.</param>
        /// <param name="valueName">Name of the value.</param>
        /// <returns>An <see cref="EnumerableValueProvider{TItem, TOwner}"/> instance.</returns>
        public static EnumerableValueProvider<TResult, TOwner> DynamicEnumerableValueOf<TSource, TResult, TOwner>(
            this IObjectProvider<TSource, TOwner> source,
            Func<TSource, IEnumerable<TResult>> valueGetFunction,
            string valueName)
        {
            source.CheckNotNull(nameof(source));
            valueGetFunction.CheckNotNull(nameof(valueGetFunction));
            valueName.CheckNotNull(nameof(valueName));

            return new EnumerableValueProvider<TResult, TOwner>(
                source.Owner,
                new DynamicObjectSource<IEnumerable<TResult>, TSource>(
                    source,
                    valueGetFunction),
                valueName);
        }

        /// <summary>
        /// Creates a lazy enumerable provider of value that is taken from <paramref name="valueGetFunction"/> with <paramref name="valueName"/> as a provider name.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <typeparam name="TOwner">The type of the owner.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="valueGetFunction">The value get function.</param>
        /// <param name="valueName">Name of the value.</param>
        /// <returns>An <see cref="EnumerableValueProvider{TItem, TOwner}"/> instance.</returns>
        public static EnumerableValueProvider<TResult, TOwner> LazyEnumerableValueOf<TSource, TResult, TOwner>(
            this IObjectProvider<TSource, TOwner> source,
            Func<TSource, IEnumerable<TResult>> valueGetFunction,
            string valueName)
        {
            source.CheckNotNull(nameof(source));
            valueGetFunction.CheckNotNull(nameof(valueGetFunction));
            valueName.CheckNotNull(nameof(valueName));

            return new EnumerableValueProvider<TResult, TOwner>(
                source.Owner,
                new LazyObjectSource<IEnumerable<TResult>, TSource>(
                    source,
                    valueGetFunction),
                valueName);
        }

        private static string ConvertToValueName(Expression expression) =>
            ObjectExpressionStringBuilder.ExpressionToString(expression);
    }
}
