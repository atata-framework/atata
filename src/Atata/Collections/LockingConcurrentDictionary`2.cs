﻿#nullable enable

namespace Atata;

internal class LockingConcurrentDictionary<TKey, TValue>
    where TKey : notnull
{
    private readonly ConcurrentDictionary<TKey, Lazy<TValue>> _dictionary = new();

    private readonly Func<TKey, Lazy<TValue>> _valueFactory;

    internal LockingConcurrentDictionary(Func<TKey, TValue> valueFactory) =>
        _valueFactory = key => new Lazy<TValue>(() => valueFactory(key));

    internal TValue GetOrAdd(TKey key) =>
        _dictionary.GetOrAdd(key, _valueFactory).Value;
}
