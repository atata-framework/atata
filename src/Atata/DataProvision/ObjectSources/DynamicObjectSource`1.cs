namespace Atata;

/// <summary>
/// Represents the dynamic object source that gets an object using function.
/// </summary>
/// <typeparam name="TObject">The type of the object.</typeparam>
public class DynamicObjectSource<TObject> : IObjectSource<TObject>
{
    private readonly Func<TObject> _objectGetFunction;

    /// <summary>
    /// Initializes a new instance of the <see cref="DynamicObjectSource{TObject}"/> class.
    /// </summary>
    /// <param name="objectGetFunction">The object get function.</param>
    public DynamicObjectSource(Func<TObject> objectGetFunction) =>
        _objectGetFunction = objectGetFunction.CheckNotNull(nameof(objectGetFunction));

    /// <inheritdoc/>
    public TObject Object =>
        _objectGetFunction.Invoke();

    /// <inheritdoc/>
    public string SourceProviderName => null;

    /// <inheritdoc/>
    public bool IsDynamic => true;
}
