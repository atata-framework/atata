namespace Atata;

/// <summary>
/// Represents the lazy object source that gets an object using function once.
/// </summary>
/// <typeparam name="TObject">The type of the object.</typeparam>
public class LazyObjectSource<TObject> : IObjectSource<TObject>
{
    private readonly Lazy<TObject> _lazyObject;

    /// <summary>
    /// Initializes a new instance of the <see cref="LazyObjectSource{TObject}"/> class.
    /// </summary>
    /// <param name="objectGetFunction">The object get function.</param>
    public LazyObjectSource(Func<TObject> objectGetFunction)
    {
        objectGetFunction.CheckNotNull(nameof(objectGetFunction));
        _lazyObject = new Lazy<TObject>(objectGetFunction);
    }

    /// <inheritdoc/>
    public TObject Object =>
        _lazyObject.Value;

    /// <inheritdoc/>
    public string SourceProviderName => null;

    /// <inheritdoc/>
    public bool IsDynamic => false;
}
