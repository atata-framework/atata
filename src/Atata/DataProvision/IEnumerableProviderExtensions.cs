﻿namespace Atata;

public static class IEnumerableProviderExtensions
{
    public static EnumerableValueProvider<TResult, TOwner> Query<TSource, TResult, TOwner>(
        this IEnumerableProvider<TSource, TOwner> source,
        string valueName,
        Func<IEnumerable<TSource>, IEnumerable<TResult>> valueGetFunction)
    {
        source.CheckNotNull(nameof(source));
        valueName.CheckNotNull(nameof(valueName));
        valueGetFunction.CheckNotNull(nameof(valueGetFunction));

        IObjectSource<IEnumerable<TResult>> valueSource = source.IsDynamic
            ? new DynamicObjectSource<IEnumerable<TResult>, IEnumerable<TSource>>(
                source,
                valueGetFunction)
            : new LazyObjectSource<IEnumerable<TResult>, IEnumerable<TSource>>(
                source,
                valueGetFunction);

        return new(source.Owner, valueSource, valueName, source.ExecutionUnit);
    }

    public static EnumerableValueProvider<TSource, TOwner> Distinct<TSource, TOwner>(
        this IEnumerableProvider<TSource, TOwner> source) =>
        source.CheckNotNull(nameof(source)).Query(
            "Distinct()",
            x => x.Distinct());

    public static EnumerableValueProvider<TSource, TOwner> Distinct<TSource, TOwner>(
        this IEnumerableProvider<TSource, TOwner> source,
        IEqualityComparer<TSource> comparer) =>
        source.CheckNotNull(nameof(source)).Query(
            $"Distinct({comparer})",
            x => x.Distinct(comparer));

    public static TSource ElementAt<TSource, TOwner>(
        this IEnumerableProvider<TSource, TOwner> source,
        int index)
    {
        TSource value = source.CheckNotNull(nameof(source)).Object.ElementAt(index);

        (value as IHasProviderName)?.SetProviderName($"ElementAt({index})");

        return value;
    }

    public static TSource ElementAtOrDefault<TSource, TOwner>(
        this IEnumerableProvider<TSource, TOwner> source,
        int index)
    {
        TSource value = source.CheckNotNull(nameof(source)).Object.ElementAtOrDefault(index);

        (value as IHasProviderName)?.SetProviderName($"ElementAtOrDefault({index})");

        return value;
    }

    public static TSource First<TSource, TOwner>(
        this IEnumerableProvider<TSource, TOwner> source)
    {
        TSource value = source.CheckNotNull(nameof(source)).Object.First();

        (value as IHasProviderName)?.SetProviderName("First()");

        return value;
    }

    public static TSource First<TSource, TOwner>(
        this IEnumerableProvider<TSource, TOwner> source,
        Expression<Func<TSource, bool>> predicate)
    {
        var predicateFunction = predicate.CheckNotNull(nameof(predicate)).Compile();

        TSource value = source.CheckNotNull(nameof(source)).Object.First(predicateFunction);

        (value as IHasProviderName)?.SetProviderName($"First({ConvertToString(predicate)})");

        return value;
    }

    public static TSource FirstOrDefault<TSource, TOwner>(
        this IEnumerableProvider<TSource, TOwner> source)
    {
        TSource value = source.CheckNotNull(nameof(source)).Object.FirstOrDefault();

        (value as IHasProviderName)?.SetProviderName("FirstOrDefault()");

        return value;
    }

    public static TSource FirstOrDefault<TSource, TOwner>(
        this IEnumerableProvider<TSource, TOwner> source,
        Expression<Func<TSource, bool>> predicate)
    {
        var predicateFunction = predicate.CheckNotNull(nameof(predicate)).Compile();

        TSource value = source.CheckNotNull(nameof(source)).Object.FirstOrDefault(predicateFunction);

        (value as IHasProviderName)?.SetProviderName($"FirstOrDefault({ConvertToString(predicate)})");

        return value;
    }

    public static TSource Last<TSource, TOwner>(
        this IEnumerableProvider<TSource, TOwner> source)
    {
        TSource value = source.CheckNotNull(nameof(source)).Object.Last();

        (value as IHasProviderName)?.SetProviderName("Last()");

        return value;
    }

    public static TSource Last<TSource, TOwner>(
        this IEnumerableProvider<TSource, TOwner> source,
        Expression<Func<TSource, bool>> predicate)
    {
        var predicateFunction = predicate.CheckNotNull(nameof(predicate)).Compile();

        TSource value = source.CheckNotNull(nameof(source)).Object.Last(predicateFunction);

        (value as IHasProviderName)?.SetProviderName($"Last({ConvertToString(predicate)})");

        return value;
    }

    public static TSource LastOrDefault<TSource, TOwner>(
        this IEnumerableProvider<TSource, TOwner> source)
    {
        TSource value = source.CheckNotNull(nameof(source)).Object.LastOrDefault();

        (value as IHasProviderName)?.SetProviderName("LastOrDefault()");

        return value;
    }

    public static TSource LastOrDefault<TSource, TOwner>(
        this IEnumerableProvider<TSource, TOwner> source,
        Expression<Func<TSource, bool>> predicate)
    {
        var predicateFunction = predicate.CheckNotNull(nameof(predicate)).Compile();

        TSource value = source.CheckNotNull(nameof(source)).Object.LastOrDefault(predicateFunction);

        (value as IHasProviderName)?.SetProviderName($"LastOrDefault({ConvertToString(predicate)})");

        return value;
    }

    public static EnumerableValueProvider<TResult, TOwner> Select<TSource, TResult, TOwner>(
        this IEnumerableProvider<TSource, TOwner> source,
        Expression<Func<TSource, TResult>> selector)
    {
        var selectorFunction = selector.CheckNotNull(nameof(selector)).Compile();

        return source.CheckNotNull(nameof(source)).Query(
            $"Select({ConvertToString(selector)})",
            x => x.Select(selectorFunction));
    }

    public static EnumerableValueProvider<TResult, TOwner> Select<TSource, TResult, TOwner>(
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
        TSource value = source.CheckNotNull(nameof(source)).Object.Single();

        (value as IHasProviderName)?.SetProviderName("Single()");

        return value;
    }

    public static TSource Single<TSource, TOwner>(
        this IEnumerableProvider<TSource, TOwner> source,
        Expression<Func<TSource, bool>> predicate)
    {
        var predicateFunction = predicate.CheckNotNull(nameof(predicate)).Compile();

        TSource value = source.CheckNotNull(nameof(source)).Object.Single(predicateFunction);

        (value as IHasProviderName)?.SetProviderName($"Single({ConvertToString(predicate)})");

        return value;
    }

    public static TSource SingleOrDefault<TSource, TOwner>(
        this IEnumerableProvider<TSource, TOwner> source)
    {
        TSource value = source.CheckNotNull(nameof(source)).Object.SingleOrDefault();

        (value as IHasProviderName)?.SetProviderName("SingleOrDefault()");

        return value;
    }

    public static TSource SingleOrDefault<TSource, TOwner>(
        this IEnumerableProvider<TSource, TOwner> source,
        Expression<Func<TSource, bool>> predicate)
    {
        var predicateFunction = predicate.CheckNotNull(nameof(predicate)).Compile();

        TSource value = source.CheckNotNull(nameof(source)).Object.SingleOrDefault(predicateFunction);

        (value as IHasProviderName)?.SetProviderName($"SingleOrDefault({ConvertToString(predicate)})");

        return value;
    }

    public static EnumerableValueProvider<TSource, TOwner> Skip<TSource, TOwner>(
        this IEnumerableProvider<TSource, TOwner> source,
        int count)
        =>
        source.CheckNotNull(nameof(source)).Query(
            $"Skip({count})",
            x => x.Skip(count));

    public static EnumerableValueProvider<TSource, TOwner> SkipWhile<TSource, TOwner>(
        this IEnumerableProvider<TSource, TOwner> source,
        Expression<Func<TSource, bool>> predicate)
    {
        var predicateFunction = predicate.CheckNotNull(nameof(predicate)).Compile();

        return source.CheckNotNull(nameof(source)).Query(
            $"SkipWhile({ConvertToString(predicate)})",
            x => x.SkipWhile(predicateFunction));
    }

    public static EnumerableValueProvider<TSource, TOwner> SkipWhile<TSource, TOwner>(
        this IEnumerableProvider<TSource, TOwner> source,
        Expression<Func<TSource, int, bool>> predicate)
    {
        var predicateFunction = predicate.CheckNotNull(nameof(predicate)).Compile();

        return source.CheckNotNull(nameof(source)).Query(
            $"SkipWhile({ConvertToString(predicate)})",
            x => x.SkipWhile(predicateFunction));
    }

    public static EnumerableValueProvider<TSource, TOwner> Take<TSource, TOwner>(
        this IEnumerableProvider<TSource, TOwner> source,
        int count)
        =>
        source.CheckNotNull(nameof(source)).Query(
            $"Take({count})",
            x => x.Take(count));

    public static EnumerableValueProvider<TSource, TOwner> TakeWhile<TSource, TOwner>(
        this IEnumerableProvider<TSource, TOwner> source,
        Expression<Func<TSource, bool>> predicate)
    {
        var predicateFunction = predicate.CheckNotNull(nameof(predicate)).Compile();

        return source.CheckNotNull(nameof(source)).Query(
            $"TakeWhile({ConvertToString(predicate)})",
            x => x.TakeWhile(predicateFunction));
    }

    public static EnumerableValueProvider<TSource, TOwner> TakeWhile<TSource, TOwner>(
        this IEnumerableProvider<TSource, TOwner> source,
        Expression<Func<TSource, int, bool>> predicate)
    {
        var predicateFunction = predicate.CheckNotNull(nameof(predicate)).Compile();

        return source.CheckNotNull(nameof(source)).Query(
            $"TakeWhile({ConvertToString(predicate)})",
            x => x.TakeWhile(predicateFunction));
    }

    public static EnumerableValueProvider<TSource, TOwner> Where<TSource, TOwner>(
        this IEnumerableProvider<TSource, TOwner> source,
        Expression<Func<TSource, int, bool>> predicate)
    {
        var predicateFunction = predicate.CheckNotNull(nameof(predicate)).Compile();

        return source.CheckNotNull(nameof(source)).Query(
            $"Where({ConvertToString(predicate)})",
            x => x.Where(predicateFunction));
    }

    public static EnumerableValueProvider<TSource, TOwner> Where<TSource, TOwner>(
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
