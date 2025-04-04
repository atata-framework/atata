namespace Atata;

internal static class DictionaryExtensions
{
    internal static TValue GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, Func<TValue> valueFactory)
    {
        if (dictionary.TryGetValue(key, out var cachedValue))
        {
            return cachedValue;
        }
        else
        {
            TValue value = valueFactory.Invoke();

            dictionary[key] = value;

            return value;
        }
    }
}
