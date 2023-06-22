namespace Atata
{
    /// <summary>
    /// Provides a set of object extension methods that wrap the object with the <see cref="Subject{TObject}"/> class.
    /// </summary>
    public static class SubjectObjectExtensions
    {
        /// <summary>
        /// Creates a new <see cref="Subject{TObject}"/> instance that wraps the <paramref name="source"/> with the <c>"sut"</c> provider name.
        /// </summary>
        /// <typeparam name="T">The type of the source.</typeparam>
        /// <param name="source">The instance to wrap with <see cref="Subject{TObject}"/>.</param>
        /// <returns>A new <see cref="Subject{TObject}"/> instance.</returns>
        public static Subject<T> ToSutSubject<T>(this T source) =>
            source.ToSubject("sut");

        /// <summary>
        /// Creates a new <see cref="Subject{TObject}"/> instance that wraps the <paramref name="source"/> with the <c>"result"</c> provider name.
        /// </summary>
        /// <typeparam name="T">The type of the source.</typeparam>
        /// <param name="source">The instance to wrap with <see cref="Subject{TObject}"/>.</param>
        /// <returns>A new <see cref="Subject{TObject}"/> instance.</returns>
        public static Subject<T> ToResultSubject<T>(this T source) =>
            source.ToSubject("result");

        /// <summary>
        /// Creates a new <see cref="Subject{TObject}"/> instance that wraps the <paramref name="source"/> with the default provider name.
        /// </summary>
        /// <typeparam name="T">The type of the source.</typeparam>
        /// <param name="source">The instance to wrap with <see cref="Subject{TObject}"/>.</param>
        /// <returns>A new <see cref="Subject{TObject}"/> instance.</returns>
        public static Subject<T> ToSubject<T>(this T source) =>
            new(source);

        /// <summary>
        /// Creates a new <see cref="Subject{TObject}"/> instance that wraps the <paramref name="source"/> with the specified <paramref name="providerName"/>.
        /// </summary>
        /// <typeparam name="T">The type of the source.</typeparam>
        /// <param name="source">The instance to wrap with <see cref="Subject{TObject}"/>.</param>
        /// <param name="providerName">Name of the provider.</param>
        /// <returns>A new <see cref="Subject{TObject}"/> instance.</returns>
        public static Subject<T> ToSubject<T>(this T source, string providerName) =>
            new(source, providerName);
    }
}
