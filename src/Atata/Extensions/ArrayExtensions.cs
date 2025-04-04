namespace Atata;

internal static class ArrayExtensions
{
    internal static TSource? FirstOrDefault<TSource>(this TSource[] source, Predicate<TSource> predicate) =>
        Array.Find(source, predicate);

    internal static bool Any<TSource>(this TSource[] source, Predicate<TSource> predicate) =>
        Array.Exists(source, predicate);

    internal static bool All<TSource>(this TSource[] source, Predicate<TSource> predicate) =>
        Array.TrueForAll(source, predicate);
}
