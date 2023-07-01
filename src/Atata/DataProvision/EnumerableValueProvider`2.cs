namespace Atata;

/// <summary>
/// Represents the value provider class that wraps enumerable of <typeparamref name="TItem"/> objects and is hosted in <typeparamref name="TOwner"/> object.
/// </summary>
/// <typeparam name="TItem">The type of the enumerable item.</typeparam>
/// <typeparam name="TOwner">The type of the owner.</typeparam>
public class EnumerableValueProvider<TItem, TOwner> :
    ValueProvider<IEnumerable<TItem>, TOwner>,
    IEnumerableProvider<TItem, TOwner>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EnumerableValueProvider{TItem, TOwner}"/> class.
    /// </summary>
    /// <param name="owner">The owner.</param>
    /// <param name="objectSource">The object source.</param>
    /// <param name="providerName">Name of the provider.</param>
    public EnumerableValueProvider(
        TOwner owner,
        IObjectSource<IEnumerable<TItem>> objectSource,
        string providerName)
        : base(owner, objectSource, providerName)
    {
    }

    /// <inheritdoc/>
    protected override IEnumerable<TItem> Object =>
        base.Object.Select((item, index) =>
        {
            (item as IHasSourceProviderName)?.SetSourceProviderName(ProviderName);
            (item as IHasProviderName)?.SetProviderName($"[{index}]");
            return item;
        });

    /// <summary>
    /// Gets the <typeparamref name="TItem"/> at the specified index.
    /// </summary>
    /// <value>
    /// The <typeparamref name="TItem"/>.
    /// </value>
    /// <param name="index">The index.</param>
    /// <returns>The found <typeparamref name="TItem"/> item.</returns>
    public TItem this[int index] =>
        Object.ElementAt(index);
}
