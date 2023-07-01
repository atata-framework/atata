namespace Atata;

/// <summary>
/// Represents the test subject that wraps <typeparamref name="TObject"/> object.
/// </summary>
/// <typeparam name="TObject">The type of the object.</typeparam>
public class Subject<TObject> : SubjectBase<TObject, Subject<TObject>>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Subject{TObject}"/> class
    /// with the default <c>"subject"</c> provider name that is taken from <see cref="Subject.DefaultSubjectName"/> property.
    /// </summary>
    /// <param name="source">The source object.</param>
    public Subject(TObject source)
        : this(new StaticObjectSource<TObject>(source))
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Subject{TObject}"/> class.
    /// </summary>
    /// <param name="source">The source object.</param>
    /// <param name="providerName">Name of the provider.</param>
    public Subject(TObject source, string providerName)
        : this(new StaticObjectSource<TObject>(source), providerName)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Subject{TObject}"/> class
    /// with the default <c>"subject"</c> provider name that is taken from <see cref="Subject.DefaultSubjectName"/> property.
    /// </summary>
    /// <param name="objectSource">The object source.</param>
    public Subject(IObjectSource<TObject> objectSource)
        : this(objectSource, Subject.DefaultSubjectName)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Subject{TObject}"/> class.
    /// </summary>
    /// <param name="objectSource">The object source.</param>
    /// <param name="providerName">Name of the provider.</param>
    public Subject(IObjectSource<TObject> objectSource, string providerName)
        : base(objectSource, providerName)
    {
    }
}
