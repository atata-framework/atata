namespace Atata;

/// <summary>
/// Represents the test subject that wraps <typeparamref name="TObject"/> object, which should be <see cref="IDisposable"/>.
/// </summary>
/// <typeparam name="TObject">The type of the object.</typeparam>
public sealed class DisposableSubject<TObject> : SubjectBase<TObject, DisposableSubject<TObject>>, IDisposable
    where TObject : IDisposable
{
    private bool _isDisposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="DisposableSubject{TObject}"/> class
    /// with the default <c>"subject"</c> provider name that is taken from <see cref="Subject.DefaultSubjectName"/> property.
    /// </summary>
    /// <param name="source">The source object.</param>
    public DisposableSubject(TObject source)
        : this(new StaticObjectSource<TObject>(source))
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DisposableSubject{TObject}"/> class.
    /// </summary>
    /// <param name="source">The source object.</param>
    /// <param name="providerName">Name of the provider.</param>
    public DisposableSubject(TObject source, string providerName)
        : this(new StaticObjectSource<TObject>(source), providerName)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DisposableSubject{TObject}"/> class
    /// with the default <c>"subject"</c> provider name that is taken from <see cref="Subject.DefaultSubjectName"/> property.
    /// </summary>
    /// <param name="objectSource">The object source.</param>
    public DisposableSubject(IObjectSource<TObject> objectSource)
        : this(objectSource, Subject.DefaultSubjectName)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DisposableSubject{TObject}"/> class.
    /// </summary>
    /// <param name="objectSource">The object source.</param>
    /// <param name="providerName">Name of the provider.</param>
    public DisposableSubject(IObjectSource<TObject> objectSource, string providerName)
        : base(objectSource, providerName)
    {
    }

    /// <summary>
    /// Disposes this object together with the associated source object.
    /// </summary>
    public void Dispose()
    {
        if (!_isDisposed)
        {
            Object.Dispose();

            _isDisposed = true;
        }

        GC.SuppressFinalize(this);
    }
}
