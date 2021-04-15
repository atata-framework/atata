using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Atata
{
    public static class IEnumerableProviderExtensions
    {
        public static EnumerableProvider<TResult, TOwner> Query<TSource, TResult, TOwner>(
            this IEnumerableProvider<TSource, TOwner> source,
            string valueName,
            Func<IEnumerable<TSource>, IEnumerable<TResult>> valueGetFunction)
        {
            source.CheckNotNull(nameof(source));
            valueName.CheckNotNull(nameof(valueName));
            valueGetFunction.CheckNotNull(nameof(valueGetFunction));

            IObjectSource<IEnumerable<TResult>> valueSource = source.IsValueDynamic
                ? new DynamicObjectSource<IEnumerable<TResult>, IEnumerable<TSource>>(
                    source,
                    valueGetFunction)
                : (IObjectSource<IEnumerable<TResult>>)new LazyObjectSource<IEnumerable<TResult>, IEnumerable<TSource>>(
                    source,
                    valueGetFunction);

            return new EnumerableProvider<TResult, TOwner>(source.Owner, valueSource, valueName);
        }

        public static EnumerableProvider<TSource, TOwner> Distinct<TSource, TOwner>(
            this IEnumerableProvider<TSource, TOwner> source)
            =>
            source.CheckNotNull(nameof(source)).Query(
                "Distinct()",
                x => x.Distinct());

        public static EnumerableProvider<TSource, TOwner> Distinct<TSource, TOwner>(
            this IEnumerableProvider<TSource, TOwner> source,
            IEqualityComparer<TSource> comparer)
            =>
            source.CheckNotNull(nameof(source)).Query(
                $"Distinct({comparer})",
                x => x.Distinct(comparer));

        public static TSource ElementAt<TSource, TOwner>(
            this IEnumerableProvider<TSource, TOwner> source,
            int index)
        {
            TSource value = source.CheckNotNull(nameof(source)).Value.ElementAt(index);

            (value as IHasProviderName)?.SetProviderName($"ElementAt({index})");

            return value;
        }

        public static TSource ElementAtOrDefault<TSource, TOwner>(
            this IEnumerableProvider<TSource, TOwner> source,
            int index)
        {
            TSource value = source.CheckNotNull(nameof(source)).Value.ElementAtOrDefault(index);

            (value as IHasProviderName)?.SetProviderName($"ElementAtOrDefault({index})");

            return value;
        }

        public static TSource First<TSource, TOwner>(
            this IEnumerableProvider<TSource, TOwner> source)
        {
            TSource value = source.CheckNotNull(nameof(source)).Value.First();

            (value as IHasProviderName)?.SetProviderName("First()");

            return value;
        }

        public static TSource First<TSource, TOwner>(
            this IEnumerableProvider<TSource, TOwner> source,
            Expression<Func<TSource, bool>> predicate)
        {
            var predicateFunction = predicate.CheckNotNull(nameof(predicate)).Compile();

            TSource value = source.CheckNotNull(nameof(source)).Value.First(predicateFunction);

            (value as IHasProviderName)?.SetProviderName($"First({ConvertToString(predicate)})");

            return value;
        }

        public static TSource FirstOrDefault<TSource, TOwner>(
            this IEnumerableProvider<TSource, TOwner> source)
        {
            TSource value = source.CheckNotNull(nameof(source)).Value.FirstOrDefault();

            (value as IHasProviderName)?.SetProviderName("FirstOrDefault()");

            return value;
        }

        public static TSource FirstOrDefault<TSource, TOwner>(
            this IEnumerableProvider<TSource, TOwner> source,
            Expression<Func<TSource, bool>> predicate)
        {
            var predicateFunction = predicate.CheckNotNull(nameof(predicate)).Compile();

            TSource value = source.CheckNotNull(nameof(source)).Value.FirstOrDefault(predicateFunction);

            (value as IHasProviderName)?.SetProviderName($"FirstOrDefault({ConvertToString(predicate)})");

            return value;
        }

        public static TSource Last<TSource, TOwner>(
            this IEnumerableProvider<TSource, TOwner> source)
        {
            TSource value = source.CheckNotNull(nameof(source)).Value.Last();

            (value as IHasProviderName)?.SetProviderName("Last()");

            return value;
        }

        public static TSource Last<TSource, TOwner>(
            this IEnumerableProvider<TSource, TOwner> source,
            Expression<Func<TSource, bool>> predicate)
        {
            var predicateFunction = predicate.CheckNotNull(nameof(predicate)).Compile();

            TSource value = source.CheckNotNull(nameof(source)).Value.Last(predicateFunction);

            (value as IHasProviderName)?.SetProviderName($"Last({ConvertToString(predicate)})");

            return value;
        }

        public static TSource LastOrDefault<TSource, TOwner>(
            this IEnumerableProvider<TSource, TOwner> source)
        {
            TSource value = source.CheckNotNull(nameof(source)).Value.LastOrDefault();

            (value as IHasProviderName)?.SetProviderName("LastOrDefault()");

            return value;
        }

        public static TSource LastOrDefault<TSource, TOwner>(
            this IEnumerableProvider<TSource, TOwner> source,
            Expression<Func<TSource, bool>> predicate)
        {
            var predicateFunction = predicate.CheckNotNull(nameof(predicate)).Compile();

            TSource value = source.CheckNotNull(nameof(source)).Value.LastOrDefault(predicateFunction);

            (value as IHasProviderName)?.SetProviderName($"LastOrDefault({ConvertToString(predicate)})");

            return value;
        }

        public static EnumerableProvider<TResult, TOwner> Select<TSource, TResult, TOwner>(
            this IEnumerableProvider<TSource, TOwner> source,
            Expression<Func<TSource, TResult>> selector)
        {
            var selectorFunction = selector.CheckNotNull(nameof(selector)).Compile();

            return source.CheckNotNull(nameof(source)).Query(
                $"Select({ConvertToString(selector)})",
                x => x.Select(selectorFunction));
        }

        public static EnumerableProvider<TResult, TOwner> Select<TSource, TResult, TOwner>(
            this IEnumerableProvider<TSource, TOwner> source,
            Expression<Func<TSource, int, TResult>> selector)
        {
            var selectorFunction = selector.CheckNotNull(nameof(selector)).Compile();

            return source.CheckNotNull(nameof(source)).Query(
                $"Select({ConvertToString(selector)})",
                x => x.Select(selectorFunction));
        }

        public static TSource Single<TSource, TOwner>(
            this IEnumerableProvider<TSource, TOwner> source)
        {
            TSource value = source.CheckNotNull(nameof(source)).Value.Single();

            (value as IHasProviderName)?.SetProviderName("Single()");

            return value;
        }

        public static TSource Single<TSource, TOwner>(
            this IEnumerableProvider<TSource, TOwner> source,
            Expression<Func<TSource, bool>> predicate)
        {
            var predicateFunction = predicate.CheckNotNull(nameof(predicate)).Compile();

            TSource value = source.CheckNotNull(nameof(source)).Value.Single(predicateFunction);

            (value as IHasProviderName)?.SetProviderName($"Single({ConvertToString(predicate)})");

            return value;
        }

        public static TSource SingleOrDefault<TSource, TOwner>(
            this IEnumerableProvider<TSource, TOwner> source)
        {
            TSource value = source.CheckNotNull(nameof(source)).Value.SingleOrDefault();

            (value as IHasProviderName)?.SetProviderName("SingleOrDefault()");

            return value;
        }

        public static TSource SingleOrDefault<TSource, TOwner>(
            this IEnumerableProvider<TSource, TOwner> source,
            Expression<Func<TSource, bool>> predicate)
        {
            var predicateFunction = predicate.CheckNotNull(nameof(predicate)).Compile();

            TSource value = source.CheckNotNull(nameof(source)).Value.SingleOrDefault(predicateFunction);

            (value as IHasProviderName)?.SetProviderName($"SingleOrDefault({ConvertToString(predicate)})");

            return value;
        }

        public static EnumerableProvider<TSource, TOwner> Skip<TSource, TOwner>(
            this IEnumerableProvider<TSource, TOwner> source,
            int count)
            =>
            source.CheckNotNull(nameof(source)).Query(
                $"Skip({count})",
                x => x.Skip(count));

        public static EnumerableProvider<TSource, TOwner> Take<TSource, TOwner>(
            this IEnumerableProvider<TSource, TOwner> source,
            int count)
            =>
            source.CheckNotNull(nameof(source)).Query(
                $"Take({count})",
                x => x.Take(count));

        public static EnumerableProvider<TSource, TOwner> Where<TSource, TOwner>(
            this IEnumerableProvider<TSource, TOwner> source,
            Expression<Func<TSource, int, bool>> predicate)
        {
            var predicateFunction = predicate.CheckNotNull(nameof(predicate)).Compile();

            return source.CheckNotNull(nameof(source)).Query(
                $"Where({ConvertToString(predicate)})",
                x => x.Where(predicateFunction));
        }

        public static EnumerableProvider<TSource, TOwner> Where<TSource, TOwner>(
            this IEnumerableProvider<TSource, TOwner> source,
            Expression<Func<TSource, bool>> predicate)
        {
            var predicateFunction = predicate.CheckNotNull(nameof(predicate)).Compile();

            return source.CheckNotNull(nameof(source)).Query(
                $"Where({ConvertToString(predicate)})",
                x => x.Where(predicateFunction));
        }

        private static string ConvertToString(Expression expression) =>
            ImprovedExpressionStringBuilder.ExpressionToString(expression);
    }
}
