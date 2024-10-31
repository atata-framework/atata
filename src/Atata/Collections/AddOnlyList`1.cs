namespace Atata;

/// <summary>
/// Represents a strongly typed list of objects that can be accessed by index.
/// The list is thread-safe.
/// Only provides a method to add items.
/// Other regular list mutations are not supported.
/// </summary>
/// <typeparam name="T">The type of elements in the list.</typeparam>
public class AddOnlyList<T> : IReadOnlyList<T>
{
    private readonly List<T> _innerList;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddOnlyList{T}"/> class
    /// that is empty and has the default initial capacity.
    /// </summary>
    public AddOnlyList() =>
        _innerList = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="AddOnlyList{T}"/> class
    /// that contains elements copied from the specified <paramref name="collection"/>
    /// and has sufficient capacity to accommodate the number of elements copied.
    /// </summary>
    /// <param name="collection">The collection.</param>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <see langword="null"/>.</exception>
    public AddOnlyList(IEnumerable<T> collection) =>
        _innerList = new(collection);

    /// <summary>
    /// Initializes a new instance of the <see cref="AddOnlyList{T}"/> class
    /// that is empty and has the specified initial capacity.
    /// </summary>
    /// <param name="capacity">The number of elements that the new list can initially store.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity"/> is less than <c>0</c>.</exception>
    public AddOnlyList(int capacity) =>
        _innerList = new(capacity);

    /// <summary>
    /// Gets the number of elements in the list.
    /// </summary>
    public int Count =>
        _innerList.Count;

    /// <summary>
    /// Gets the element at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the element to get.</param>
    /// <returns>The element at the specified index.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="index"/> is less than <c>0</c>.
    /// -or-
    /// <paramref name="index"/> is equal to or greater than <see cref="Count"/>.
    /// </exception>
    public T this[int index] =>
        _innerList[index];

    /// <summary>
    /// Adds an object to the end of the list.
    /// </summary>
    /// <param name="item">
    /// The object to be added to the end of the list.
    /// The value can be <see langword="null"/> for reference types.
    /// </param>
    public void Add(T item)
    {
        lock (_innerList)
        {
            _innerList.Add(item);
        }
    }

    /// <inheritdoc/>
    public IEnumerator<T> GetEnumerator()
    {
        for (int i = 0; i < _innerList.Count; i++)
        {
            yield return _innerList[i];
        }
    }

    IEnumerator IEnumerable.GetEnumerator() =>
        GetEnumerator();
}
