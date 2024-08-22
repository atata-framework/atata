namespace Atata;

/// <summary>
/// Represents a hierarchical dictionary, which can contain a parent dictionary.
/// A search of elements occurs first in this dictionary and when element is not found,
/// the search continues in the parent dictionary.
/// Parent dictionary can also be a <see cref="HierarchicalDictionary{TKey, TValue}"/>,
/// which allows building of multi-level dictionaries.
/// </summary>
/// <typeparam name="TKey">The type of keys.</typeparam>
/// <typeparam name="TValue">The type of values.</typeparam>
public class HierarchicalDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
{
    private readonly object _modificationLock = new();

    private IReadOnlyDictionary<TKey, TValue> _parentDictionary;

    private Dictionary<TKey, TValue> _thisDictionary;

    /// <summary>
    /// Initializes a new instance of the <see cref="HierarchicalDictionary{TKey, TValue}"/> class.
    /// </summary>
    /// <param name="parentDictionary">The parent dictionary, which is optional.</param>
    /// <param name="comparer">The comparer, which is optional.</param>
    public HierarchicalDictionary(
        IReadOnlyDictionary<TKey, TValue> parentDictionary = null,
        IEqualityComparer<TKey> comparer = null)
    {
        _parentDictionary = parentDictionary ?? new Dictionary<TKey, TValue>();
        comparer ??= (parentDictionary as HierarchicalDictionary<TKey, TValue>)?.Comparer
            ?? (parentDictionary as Dictionary<TKey, TValue>)?.Comparer;
        _thisDictionary = new(comparer);
    }

    /// <summary>
    /// Gets the <see cref="IEqualityComparer{T}"/> that is used to determine
    /// equality of keys for the dictionary.
    /// </summary>
    public IEqualityComparer<TKey> Comparer =>
        _thisDictionary.Comparer;

    /// <summary>
    /// Gets an enumerable collection that contains the keys in the dictionary.
    /// </summary>
    public IEnumerable<TKey> Keys =>
        Enumerate().Select(x => x.Key);

    /// <summary>
    /// Gets an enumerable collection that contains the values in the dictionary.
    /// </summary>
    public IEnumerable<TValue> Values =>
        Enumerate().Select(x => x.Value);

    /// <summary>
    /// Gets the number of elements in the dictionary.
    /// </summary>
    public int Count =>
        Enumerate().Count();

    /// <summary>
    /// Gets or sets the value associated with the specified key.
    /// </summary>
    /// <param name="key">The key of the value to get or set.</param>
    /// <returns>
    /// The value associated with the specified key. If the specified key is not found,
    /// a get operation throws a <see cref="KeyNotFoundException"/>,
    /// and a set operation creates a new element with the specified key.
    /// </returns>
    /// <exception cref="KeyNotFoundException">The given key '{key}' was not present in the dictionary.</exception>
    public TValue this[TKey key]
    {
        get => TryGetValue(key, out var value)
            ? value
            : throw new KeyNotFoundException($"The given key '{key}' was not present in the dictionary.");
        set
        {
            lock (_modificationLock)
            {
                var dictionaryCopy = CopyThisDictionary();
                dictionaryCopy[key] = value;
                _thisDictionary = dictionaryCopy;
            }
        }
    }

    /// <summary>
    /// Adds the specified key and value to the dictionary.
    /// </summary>
    /// <param name="key">The key of the element to add.</param>
    /// <param name="value">The value of the element to add. The value can be null for reference types.</param>
    public void Add(TKey key, TValue value)
    {
        lock (_modificationLock)
        {
            var dictionaryCopy = CopyThisDictionary();
            dictionaryCopy.Add(key, value);
            _thisDictionary = dictionaryCopy;
        }
    }

    /// <summary>
    /// Removes the value with the specified key from this dictionary.
    /// </summary>
    /// <param name="key">The key of the element to remove.</param>
    /// <returns><see langword="true"/> if the element is successfully found and removed; otherwise, <see langword="false"/>.</returns>
    public bool Remove(TKey key)
    {
        lock (_modificationLock)
        {
            var dictionaryCopy = CopyThisDictionary();
            bool isRemoved = dictionaryCopy.Remove(key);
            _thisDictionary = dictionaryCopy;
            return isRemoved;
        }
    }

    /// <summary>
    /// Removes all keys and values from this dictionary.
    /// </summary>
    public void Clear()
    {
        lock (_modificationLock)
        {
            if (_thisDictionary.Count > 0)
                _thisDictionary = [];
        }
    }

    internal void SetInitialValue(TKey key, TValue value) =>
        _thisDictionary[key] = value;

    internal void ChangeParentDictionary(IReadOnlyDictionary<TKey, TValue> parentDictionary) =>
        _parentDictionary = parentDictionary;

    /// <summary>
    /// Determines whether the dictionary contains an element that has the specified key.
    /// </summary>
    /// <param name="key">The key to locate.</param>
    /// <returns>
    /// <see langword="true"/> if the read-only dictionary contains an element that has the specified key; otherwise, <see langword="false"/>.
    /// </returns>
    public bool ContainsKey(TKey key) =>
        _thisDictionary.ContainsKey(key) || _parentDictionary.ContainsKey(key);

    IEnumerator IEnumerable.GetEnumerator() =>
        GetEnumerator();

    /// <inheritdoc/>
    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() =>
        Enumerate().GetEnumerator();

    private IEnumerable<KeyValuePair<TKey, TValue>> Enumerate()
    {
        var ownDictionary = _thisDictionary;
        var ownKeys = ownDictionary.Keys;

        foreach (var item in ownDictionary)
            yield return item;

        foreach (var item in _parentDictionary)
            if (!ownKeys.Contains(item.Key, ownDictionary.Comparer))
                yield return item;
    }

    /// <summary>
    /// Gets the value that is associated with the specified key.
    /// </summary>
    /// <param name="key">The key to locate.</param>
    /// <param name="value">
    /// When this method returns, the value associated with the specified key, if the key is found;
    /// otherwise, the default value for the type of the value parameter.
    /// This parameter is passed uninitialized.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the dictionary contains an element that has the specified key;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public bool TryGetValue(TKey key, out TValue value) =>
        _thisDictionary.TryGetValue(key, out value) || _parentDictionary.TryGetValue(key, out value);

    private Dictionary<TKey, TValue> CopyThisDictionary() =>
        new(_thisDictionary, Comparer);

    public Dictionary<TKey, TValue> ToDictionary()
    {
        Dictionary<TKey, TValue> dictionary = new(Comparer);

        foreach (var item in Enumerate())
            dictionary[item.Key] = item.Value;

        return dictionary;
    }
}
