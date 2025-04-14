namespace Atata;

/// <summary>
/// Represents the dynamic object source that gets an object using function.
/// Also takes an instance of <see cref="IObjectProvider{TObject}" /> of source.
/// </summary>
/// <typeparam name="TObject">The type of the object.</typeparam>
/// <typeparam name="TSource">The type of the source.</typeparam>
public class DynamicObjectSource<TObject, TSource> : IObjectSource<TObject>
{
    private readonly IObjectProvider<TSource> _sourceProvider;

    private readonly Func<TSource, TObject> _objectGetFunction;

    /// <summary>
    /// Initializes a new instance of the <see cref="DynamicObjectSource{TObject, TSource}"/> class.
    /// </summary>
    /// <param name="sourceProvider">The source provider.</param>
    /// <param name="objectGetFunction">The object get function.</param>
    public DynamicObjectSource(IObjectProvider<TSource> sourceProvider, Func<TSource, TObject> objectGetFunction)
    {
        Guard.ThrowIfNull(sourceProvider);
        Guard.ThrowIfNull(objectGetFunction);

        _sourceProvider = sourceProvider;
        _objectGetFunction = objectGetFunction;
    }

    /// <inheritdoc/>
    public TObject Object =>
        _objectGetFunction.Invoke(_sourceProvider.Object);

    /// <inheritdoc/>
    public string? SourceProviderName =>
        _sourceProvider.ProviderName;

    /// <inheritdoc/>
    public bool IsDynamic => true;
}
