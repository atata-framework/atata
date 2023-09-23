namespace Atata;

/// <summary>
/// Provides a set of extension methods for <see cref="IObjectProvider{TObject, TOwner}"/> where <c>TObject</c> is <see cref="IEnumerable{T}"/>.
/// </summary>
public static class IObjectProviderEnumerableExtensions
{
    /// <inheritdoc cref="Contains{TSource}(IObjectProvider{IEnumerable{TSource}}, IEnumerable{TSource})"/>
    public static bool Contains<TSource>(
        this IObjectProvider<IEnumerable<TSource>> source,
        params TSource[] items) =>
        source.Contains(items?.AsEnumerable());

    /// <summary>
    /// Determines whether the source sequence contains all of the specified items by using the default equality comparer.
    /// </summary>
    /// <typeparam name="TSource">The type of the source item.</typeparam>
    /// <param name="source">The source.</param>
    /// <param name="items">The items to locate.</param>
    /// <returns>
    /// <see langword="true"/> if the source sequence contains all; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool Contains<TSource>(
        this IObjectProvider<IEnumerable<TSource>> source,
        IEnumerable<TSource> items)
    {
        source.CheckNotNull(nameof(source));
        items.CheckNotNullOrEmpty(nameof(items));

        return source.Object.Intersect(items).Count() == items.Distinct().Count();
    }

    /// <inheritdoc cref="ContainsAny{TSource}(IObjectProvider{IEnumerable{TSource}}, IEnumerable{TSource})"/>
    public static bool ContainsAny<TSource>(
        this IObjectProvider<IEnumerable<TSource>> source,
        params TSource[] items) =>
        source.ContainsAny(items?.AsEnumerable());

    /// <summary>
    /// Determines whether the source sequence contains any of the specified items by using the default equality comparer.
    /// </summary>
    /// <typeparam name="TSource">The type of the source item.</typeparam>
    /// <param name="source">The source.</param>
    /// <param name="items">The items to locate.</param>
    /// <returns>
    /// <see langword="true"/> if the source sequence contains any; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool ContainsAny<TSource>(
        this IObjectProvider<IEnumerable<TSource>> source,
        IEnumerable<TSource> items)
    {
        source.CheckNotNull(nameof(source));
        items.CheckNotNullOrEmpty(nameof(items));

        return source.Object.Intersect(items).Any();
    }

    /// <summary>
    /// Executes a foreach operation on source elements.
    /// </summary>
    /// <typeparam name="TSource">The type of the source item.</typeparam>
    /// <typeparam name="TOwner">The type of the owner.</typeparam>
    /// <param name="source">The source.</param>
    /// <param name="action">The action to invoke once per iteration.</param>
    /// <returns>An owner instance.</returns>
    public static TOwner ForEach<TSource, TOwner>(
        this IObjectProvider<IEnumerable<TSource>, TOwner> source,
        Action<TSource> action)
    {
        source.CheckNotNull(nameof(source));
        action.CheckNotNull(nameof(action));

        foreach (TSource item in source.Object)
            action.Invoke(item);

        return source.Owner;
    }

    public static ValueProvider<int, TOwner> Count<TSource, TOwner>(
        this IObjectProvider<IEnumerable<TSource>, TOwner> source)
        =>
        source.ValueOf(x => x.Count(), "Count()");

    public static ValueProvider<int, TOwner> Count<TSource, TOwner>(
        this IObjectProvider<IEnumerable<TSource>, TOwner> source,
        Expression<Func<TSource, bool>> predicate)
    {
        var predicateFunction = predicate.CheckNotNull(nameof(predicate)).Compile();

        return source.ValueOf(
            x => x.Count(predicateFunction),
            $"Count({ConvertToString(predicate)})");
    }

    public static ValueProvider<IEnumerable<TSource>, TOwner> Distinct<TSource, TOwner>(
        this IObjectProvider<IEnumerable<TSource>, TOwner> source)
        =>
        source.ValueOf(
            x => x.Distinct(),
            "Distinct()");

    public static ValueProvider<IEnumerable<TSource>, TOwner> Distinct<TSource, TOwner>(
       this IObjectProvider<IEnumerable<TSource>, TOwner> source,
       IEqualityComparer<TSource> comparer)
       =>
       source.ValueOf(
           x => x.Distinct(comparer),
           $"Distinct({comparer})");

    public static ValueProvider<TSource, TOwner> ElementAt<TSource, TOwner>(
        this IObjectProvider<IEnumerable<TSource>, TOwner> source,
        int index)
        =>
        source.ValueOf(
            x => x.ElementAt(index),
            $"ElementAt({index})");

    public static ValueProvider<TSource, TOwner> ElementAtOrDefault<TSource, TOwner>(
        this IObjectProvider<IEnumerable<TSource>, TOwner> source,
        int index)
        =>
        source.ValueOf(
            x => x.ElementAtOrDefault(index),
            $"ElementAtOrDefault({index})");

    public static ValueProvider<TSource, TOwner> First<TSource, TOwner>(
        this IObjectProvider<IEnumerable<TSource>, TOwner> source)
        =>
        source.ValueOf(x => x.First(), "First()");

    public static ValueProvider<TSource, TOwner> First<TSource, TOwner>(
        this IObjectProvider<IEnumerable<TSource>, TOwner> source,
        Expression<Func<TSource, bool>> predicate)
    {
        var predicateFunction = predicate.CheckNotNull(nameof(predicate)).Compile();

        return source.ValueOf(
            x => x.First(predicateFunction),
            $"First({ConvertToString(predicate)})");
    }

    public static ValueProvider<TSource, TOwner> FirstOrDefault<TSource, TOwner>(
        this IObjectProvider<IEnumerable<TSource>, TOwner> source)
        =>
        source.ValueOf(x => x.FirstOrDefault(), "FirstOrDefault()");

    public static ValueProvider<TSource, TOwner> FirstOrDefault<TSource, TOwner>(
        this IObjectProvider<IEnumerable<TSource>, TOwner> source,
        Expression<Func<TSource, bool>> predicate)
    {
        var predicateFunction = predicate.CheckNotNull(nameof(predicate)).Compile();

        return source.ValueOf(
            x => x.FirstOrDefault(predicateFunction),
            $"FirstOrDefault({ConvertToString(predicate)})");
    }

    public static ValueProvider<TSource, TOwner> Last<TSource, TOwner>(
        this IObjectProvider<IEnumerable<TSource>, TOwner> source)
        =>
        source.ValueOf(x => x.Last(), "Last()");

    public static ValueProvider<TSource, TOwner> Last<TSource, TOwner>(
        this IObjectProvider<IEnumerable<TSource>, TOwner> source,
        Expression<Func<TSource, bool>> predicate)
    {
        var predicateFunction = predicate.CheckNotNull(nameof(predicate)).Compile();

        return source.ValueOf(
            x => x.Last(predicateFunction),
            $"Last({ConvertToString(predicate)})");
    }

    public static ValueProvider<TSource, TOwner> LastOrDefault<TSource, TOwner>(
        this IObjectProvider<IEnumerable<TSource>, TOwner> source)
        =>
        source.ValueOf(x => x.LastOrDefault(), "LastOrDefault()");

    public static ValueProvider<TSource, TOwner> LastOrDefault<TSource, TOwner>(
        this IObjectProvider<IEnumerable<TSource>, TOwner> source,
        Expression<Func<TSource, bool>> predicate)
    {
        var predicateFunction = predicate.CheckNotNull(nameof(predicate)).Compile();

        return source.ValueOf(
            x => x.LastOrDefault(predicateFunction),
            $"LastOrDefault({ConvertToString(predicate)})");
    }

    public static ValueProvider<long, TOwner> LongCount<TSource, TOwner>(
        this IObjectProvider<IEnumerable<TSource>, TOwner> source)
        =>
        source.ValueOf(x => x.LongCount(), "LongCount()");

    public static ValueProvider<long, TOwner> LongCount<TSource, TOwner>(
        this IObjectProvider<IEnumerable<TSource>, TOwner> source,
        Expression<Func<TSource, bool>> predicate)
    {
        var predicateFunction = predicate.CheckNotNull(nameof(predicate)).Compile();

        return source.ValueOf(
            x => x.LongCount(predicateFunction),
            $"LongCount({ConvertToString(predicate)})");
    }

    public static ValueProvider<TResult, TOwner> Max<TSource, TResult, TOwner>(
        this IObjectProvider<IEnumerable<TSource>, TOwner> source,
        Expression<Func<TSource, TResult>> predicate)
    {
        var predicateFunction = predicate.CheckNotNull(nameof(predicate)).Compile();

        return source.ValueOf(
            x => x.Max(predicateFunction),
            $"Max({ConvertToString(predicate)})");
    }

    public static ValueProvider<TSource, TOwner> Max<TSource, TOwner>(
        this IObjectProvider<IEnumerable<TSource>, TOwner> source)
        =>
        source.ValueOf(
            x => x.Max(),
            "Max()");

    public static ValueProvider<TResult, TOwner> Min<TSource, TResult, TOwner>(
        this IObjectProvider<IEnumerable<TSource>, TOwner> source,
        Expression<Func<TSource, TResult>> predicate)
    {
        var predicateFunction = predicate.CheckNotNull(nameof(predicate)).Compile();

        return source.ValueOf(
            x => x.Min(predicateFunction),
            $"Min({ConvertToString(predicate)})");
    }

    public static ValueProvider<TSource, TOwner> Min<TSource, TOwner>(
        this IObjectProvider<IEnumerable<TSource>, TOwner> source)
        =>
        source.ValueOf(
            x => x.Min(),
            "Min()");

    public static ValueProvider<IEnumerable<TResult>, TOwner> Select<TSource, TResult, TOwner>(
        this IObjectProvider<IEnumerable<TSource>, TOwner> source,
        Expression<Func<TSource, TResult>> selector)
    {
        var predicateFunction = selector.CheckNotNull(nameof(selector)).Compile();

        return source.ValueOf(
            x => x.Select(predicateFunction),
            $"Select({ConvertToString(selector)})");
    }

    public static ValueProvider<IEnumerable<TResult>, TOwner> Select<TSource, TResult, TOwner>(
        this IObjectProvider<IEnumerable<TSource>, TOwner> source,
        Expression<Func<TSource, int, TResult>> selector)
    {
        var predicateFunction = selector.CheckNotNull(nameof(selector)).Compile();

        return source.ValueOf(
            x => x.Select(predicateFunction),
            $"Select({ConvertToString(selector)})");
    }

    public static ValueProvider<TSource, TOwner> Single<TSource, TOwner>(
        this IObjectProvider<IEnumerable<TSource>, TOwner> source)
        =>
        source.ValueOf(x => x.Single(), "Single()");

    public static ValueProvider<TSource, TOwner> Single<TSource, TOwner>(
        this IObjectProvider<IEnumerable<TSource>, TOwner> source,
        Expression<Func<TSource, bool>> predicate)
    {
        var predicateFunction = predicate.CheckNotNull(nameof(predicate)).Compile();

        return source.ValueOf(
            x => x.Single(predicateFunction),
            $"Single({ConvertToString(predicate)})");
    }

    public static ValueProvider<TSource, TOwner> SingleOrDefault<TSource, TOwner>(
        this IObjectProvider<IEnumerable<TSource>, TOwner> source)
        =>
        source.ValueOf(x => x.SingleOrDefault(), "SingleOrDefault()");

    public static ValueProvider<TSource, TOwner> SingleOrDefault<TSource, TOwner>(
        this IObjectProvider<IEnumerable<TSource>, TOwner> source,
        Expression<Func<TSource, bool>> predicate)
    {
        var predicateFunction = predicate.CheckNotNull(nameof(predicate)).Compile();

        return source.ValueOf(
            x => x.SingleOrDefault(predicateFunction),
            $"SingleOrDefault({ConvertToString(predicate)})");
    }

    public static ValueProvider<IEnumerable<TSource>, TOwner> Skip<TSource, TOwner>(
        this IObjectProvider<IEnumerable<TSource>, TOwner> source,
        int count)
        =>
        source.ValueOf(
            x => x.Skip(count),
            $"Skip({count})");

    public static ValueProvider<IEnumerable<TSource>, TOwner> SkipWhile<TSource, TOwner>(
        this IObjectProvider<IEnumerable<TSource>, TOwner> source,
        Expression<Func<TSource, bool>> predicate)
    {
        var predicateFunction = predicate.CheckNotNull(nameof(predicate)).Compile();

        return source.ValueOf(
            x => x.SkipWhile(predicateFunction),
            $"SkipWhile({ConvertToString(predicate)})");
    }

    public static ValueProvider<IEnumerable<TSource>, TOwner> SkipWhile<TSource, TOwner>(
        this IObjectProvider<IEnumerable<TSource>, TOwner> source,
        Expression<Func<TSource, int, bool>> predicate)
    {
        var predicateFunction = predicate.CheckNotNull(nameof(predicate)).Compile();

        return source.ValueOf(
            x => x.SkipWhile(predicateFunction),
            $"SkipWhile({ConvertToString(predicate)})");
    }

    public static ValueProvider<IEnumerable<TSource>, TOwner> Take<TSource, TOwner>(
        this IObjectProvider<IEnumerable<TSource>, TOwner> source,
        int count)
        =>
        source.ValueOf(
            x => x.Take(count),
            $"Take({count})");

    public static ValueProvider<IEnumerable<TSource>, TOwner> TakeWhile<TSource, TOwner>(
        this IObjectProvider<IEnumerable<TSource>, TOwner> source,
        Expression<Func<TSource, bool>> predicate)
    {
        var predicateFunction = predicate.CheckNotNull(nameof(predicate)).Compile();

        return source.ValueOf(
            x => x.TakeWhile(predicateFunction),
            $"TakeWhile({ConvertToString(predicate)})");
    }

    public static ValueProvider<IEnumerable<TSource>, TOwner> TakeWhile<TSource, TOwner>(
        this IObjectProvider<IEnumerable<TSource>, TOwner> source,
        Expression<Func<TSource, int, bool>> predicate)
    {
        var predicateFunction = predicate.CheckNotNull(nameof(predicate)).Compile();

        return source.ValueOf(
            x => x.TakeWhile(predicateFunction),
            $"TakeWhile({ConvertToString(predicate)})");
    }

    public static TSource[] ToArray<TSource, TOwner>(
        this IObjectProvider<IEnumerable<TSource>, TOwner> source)
        =>
        source.CheckNotNull(nameof(source)).Object.ToArray();

    public static List<TSource> ToList<TSource, TOwner>(
        this IObjectProvider<IEnumerable<TSource>, TOwner> source)
        =>
        source.CheckNotNull(nameof(source)).Object.ToList();

    public static ValueProvider<IEnumerable<TSource>, TOwner> Where<TSource, TOwner>(
        this IObjectProvider<IEnumerable<TSource>, TOwner> source,
        Expression<Func<TSource, int, bool>> predicate)
    {
        var predicateFunction = predicate.CheckNotNull(nameof(predicate)).Compile();

        return source.ValueOf(
            x => x.Where(predicateFunction),
            $"Where({ConvertToString(predicate)})");
    }

    public static ValueProvider<IEnumerable<TSource>, TOwner> Where<TSource, TOwner>(
        this IObjectProvider<IEnumerable<TSource>, TOwner> source,
        Expression<Func<TSource, bool>> predicate)
    {
        var predicateFunction = predicate.CheckNotNull(nameof(predicate)).Compile();

        return source.ValueOf(
            x => x.Where(predicateFunction),
            $"Where({ConvertToString(predicate)})");
    }

    private static string ConvertToString(Expression expression) =>
        ImprovedExpressionStringBuilder.ExpressionToString(expression);
}
