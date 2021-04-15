namespace Atata
{
    /// <summary>
    /// Represents the test subject that wraps <typeparamref name="T"/> object.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    public class Subject<T> : SubjectBase<T, Subject<T>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Subject{T}"/> class
        /// with the default <c>"subject"</c> provider name that is taken from <see cref="Subject.DefaultSubjectName"/> property.
        /// </summary>
        /// <param name="source">The source object.</param>
        public Subject(T source)
            : this(new StaticObjectSource<T>(source))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Subject{T}"/> class.
        /// </summary>
        /// <param name="source">The source object.</param>
        /// <param name="providerName">Name of the provider.</param>
        public Subject(T source, string providerName)
            : this(new StaticObjectSource<T>(source), providerName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Subject{T}"/> class
        /// with the default <c>"subject"</c> provider name that is taken from <see cref="Subject.DefaultSubjectName"/> property.
        /// </summary>
        /// <param name="objectSource">The object source.</param>
        public Subject(IObjectSource<T> objectSource)
            : this(objectSource, Subject.DefaultSubjectName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Subject{T}"/> class.
        /// </summary>
        /// <param name="objectSource">The object source.</param>
        /// <param name="providerName">Name of the provider.</param>
        public Subject(IObjectSource<T> objectSource, string providerName)
            : base(objectSource, providerName)
        {
        }
    }
}
