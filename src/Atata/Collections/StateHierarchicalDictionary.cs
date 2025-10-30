namespace Atata;

/// <summary>
/// Represents a state hierarchical dictionary, which can contain a parent dictionary.
/// A state dictionary primarily serves as a container of entities and other objects,
/// which are stored mostly by type as a key.
/// A search of elements occurs first in this dictionary and when element is not found,
/// the search continues in the parent dictionary.
/// Parent dictionary can also be a <see cref="StateHierarchicalDictionary"/>,
/// which allows building of multi-level dictionaries.
/// </summary>
public sealed class StateHierarchicalDictionary : ObjectHierarchicalDictionary<string>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StateHierarchicalDictionary"/> class.
    /// </summary>
    /// <param name="parentDictionary">The parent dictionary, which is optional.</param>
    /// <param name="comparer">The comparer, which is optional.</param>
    public StateHierarchicalDictionary(
        IReadOnlyDictionary<string, object?>? parentDictionary = null,
        IEqualityComparer<string>? comparer = null)
        : base(parentDictionary, comparer)
    {
    }

    /// <summary>
    /// Resolves the type key.
    /// </summary>
    /// <typeparam name="T">The type.</typeparam>
    /// <returns>A key.</returns>
    public static string ResolveTypeKey<T>() =>
        typeof(T).FullName;

    /// <summary>
    /// Gets the value associated with the <typeparamref name="TValue"/> type full name as a key.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <returns>
    /// The value associated with the key.
    /// If the key is not found, the method throws a <see cref="KeyNotFoundException"/>.
    /// </returns>
    /// <exception cref="KeyNotFoundException">The given key '{key}' was not present in the dictionary.</exception>
    public TValue Get<TValue>() =>
        Get<TValue>(ResolveTypeKey<TValue>());

    /// <summary>
    /// Gets the value that is associated with the <typeparamref name="TValue"/> type full name as a key.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="value">
    /// When this method returns, the value associated with the specified key, if the key is found;
    /// otherwise, the default value for the type of the value parameter.
    /// This parameter is passed uninitialized.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the dictionary contains an element that has the key;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public bool TryGetValue<TValue>([MaybeNullWhen(false)] out TValue value) =>
        TryGetValue(ResolveTypeKey<TValue>(), out value);

    /// <summary>
    /// Sets the value associated with the <typeparamref name="TValue"/> type full name as a key.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="value">The value.</param>
    public void Set<TValue>(TValue value) =>
        Set(ResolveTypeKey<TValue>(), value);
}
