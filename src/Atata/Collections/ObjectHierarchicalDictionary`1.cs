namespace Atata;

/// <summary>
/// Represents a hierarchical dictionary of objects, which can contain a parent dictionary.
/// A search of elements occurs first in this dictionary and when element is not found,
/// the search continues in the parent dictionary.
/// Parent dictionary can also be an <see cref="ObjectHierarchicalDictionary{TKey}"/>,
/// which allows building of multi-level dictionaries.
/// </summary>
/// <typeparam name="TKey">The type of keys.</typeparam>
public class ObjectHierarchicalDictionary<TKey> : HierarchicalDictionary<TKey, object?>
    where TKey : notnull
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ObjectHierarchicalDictionary{TKey}"/> class.
    /// </summary>
    /// <param name="parentDictionary">The parent dictionary, which is optional.</param>
    /// <param name="comparer">The comparer, which is optional.</param>
    public ObjectHierarchicalDictionary(
        IReadOnlyDictionary<TKey, object?>? parentDictionary = null,
        IEqualityComparer<TKey>? comparer = null)
        : base(parentDictionary, comparer)
    {
    }

    /// <summary>
    /// Gets the value associated with the specified key.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="key">The key of the value to get.</param>
    /// <returns>
    /// The value associated with the specified key.
    /// If the specified key is not found, the method throws a <see cref="KeyNotFoundException"/>.
    /// </returns>
    /// <exception cref="KeyNotFoundException">The given key '{key}' was not present in the dictionary.</exception>
    public TValue Get<TValue>(TKey key) =>
        (TValue)this[key]!;

    /// <inheritdoc cref="HierarchicalDictionary{TKey, TValue}.TryGetValue(TKey, out TValue)"/>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public bool TryGetValue<TValue>(TKey key, [MaybeNullWhen(false)] out TValue value)
    {
        if (base.TryGetValue(key, out object? result))
        {
            value = (TValue)result!;
            return true;
        }
        else
        {
            value = default;
            return false;
        }
    }

    /// <summary>
    /// Sets the value associated with the specified key.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    public void Set<TValue>(TKey key, TValue value) =>
        this[key] = value;
}
