namespace Atata;

/// <summary>
/// Represents the value provider class that wraps <typeparamref name="TValue"/> and is hosted in <typeparamref name="TOwner"/> object.
/// </summary>
/// <typeparam name="TValue">The type of the value.</typeparam>
/// <typeparam name="TOwner">The type of the owner.</typeparam>
public class ValueProvider<TValue, TOwner> : ObjectProvider<TValue, TOwner>
{
    private readonly TOwner _owner;

    /// <summary>
    /// Initializes a new instance of the <see cref="ValueProvider{TValue, TOwner}"/> class.
    /// </summary>
    /// <param name="owner">The owner.</param>
    /// <param name="objectSource">The object source.</param>
    /// <param name="providerName">Name of the provider.</param>
    /// <param name="executionUnit">The execution unit, which can be <see langword="null"/>.</param>
    public ValueProvider(
        TOwner owner,
        IObjectSource<TValue> objectSource,
        string providerName,
        IAtataExecutionUnit executionUnit = null)
        : base(objectSource, providerName, executionUnit)
        =>
        _owner = owner.CheckNotNull(nameof(owner));

    /// <inheritdoc/>
    protected override TOwner Owner => _owner;

    /// <summary>
    /// Gets the value.
    /// </summary>
    public TValue Value => Object;
}
