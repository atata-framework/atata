namespace Atata;

/// <summary>
/// Represents the lazy object source that gets an object using function once.
/// Also takes an instance of <see cref="IObjectProvider{TObject}" /> of source.
/// </summary>
/// <typeparam name="TObject">The type of the object.</typeparam>
/// <typeparam name="TSource">The type of the source.</typeparam>
public class LazyObjectSource<TObject, TSource> : IObjectSource<TObject>
{
    private readonly IObjectProvider<TSource> _sourceProvider;

    private readonly Lazy<TObject> _lazyObject;

    /// <summary>
    /// Initializes a new instance of the <see cref="LazyObjectSource{TObject, TSource}"/> class.
    /// </summary>
    /// <param name="sourceProvider">The source provider.</param>
    /// <param name="objectGetFunction">The object get function.</param>
    public LazyObjectSource(IObjectProvider<TSource> sourceProvider, Func<TSource, TObject> objectGetFunction)
    {
        Guard.ThrowIfNull(sourceProvider);
        Guard.ThrowIfNull(objectGetFunction);

        _sourceProvider = sourceProvider;

        _lazyObject = new(() => objectGetFunction.Invoke(_sourceProvider.Object));
        _sourceProvider = sourceProvider;
    }

    /// <inheritdoc/>
    public TObject Object =>
        _lazyObject.Value;

    /// <inheritdoc/>
    public string? SourceProviderName =>
        _sourceProvider.ProviderName;

    /// <inheritdoc/>
    public bool IsDynamic => false;
}
