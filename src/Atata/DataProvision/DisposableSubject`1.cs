using System;

namespace Atata
{
    /// <summary>
    /// Represents the test subject that wraps <typeparamref name="T"/> object, which should be <see cref="IDisposable"/>.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    public sealed class DisposableSubject<T> : SubjectBase<T, DisposableSubject<T>>, IDisposable
        where T : IDisposable
    {
        private bool isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="DisposableSubject{T}"/> class
        /// with the default <c>"subject"</c> provider name that is taken from <see cref="Subject.DefaultSubjectName"/> property.
        /// </summary>
        /// <param name="source">The source object.</param>
        public DisposableSubject(T source)
            : this(new StaticObjectSource<T>(source))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DisposableSubject{T}"/> class.
        /// </summary>
        /// <param name="source">The source object.</param>
        /// <param name="providerName">Name of the provider.</param>
        public DisposableSubject(T source, string providerName)
            : this(new StaticObjectSource<T>(source), providerName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DisposableSubject{T}"/> class
        /// with the default <c>"subject"</c> provider name that is taken from <see cref="Subject.DefaultSubjectName"/> property.
        /// </summary>
        /// <param name="objectSource">The object source.</param>
        public DisposableSubject(IObjectSource<T> objectSource)
            : this(objectSource, Subject.DefaultSubjectName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DisposableSubject{T}"/> class.
        /// </summary>
        /// <param name="objectSource">The object source.</param>
        /// <param name="providerName">Name of the provider.</param>
        public DisposableSubject(IObjectSource<T> objectSource, string providerName)
            : base(objectSource, providerName)
        {
        }

        /// <summary>
        /// Disposes this object together with the associated source object.
        /// </summary>
        public void Dispose()
        {
            if (!isDisposed)
            {
                Value.Dispose();

                isDisposed = true;
            }

            GC.SuppressFinalize(this);
        }
    }
}
