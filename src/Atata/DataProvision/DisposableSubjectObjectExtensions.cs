using System;

namespace Atata
{
    /// <summary>
    /// Provides a set of object extension methods that wrap the <see cref="IDisposable"/> object with the <see cref="DisposableSubject{T}"/> class.
    /// </summary>
    public static class DisposableSubjectObjectExtensions
    {
        /// <summary>
        /// Creates a new <see cref="DisposableSubject{T}"/> instance that wraps the <paramref name="source"/> with the <c>"sut"</c> provider name.
        /// </summary>
        /// <typeparam name="T">The type of the source.</typeparam>
        /// <param name="source">The instance to wrap with <see cref="DisposableSubject{T}"/>.</param>
        /// <returns>A new <see cref="DisposableSubject{T}"/> instance.</returns>
        public static DisposableSubject<T> ToSutDisposableSubject<T>(this T source)
            where T : IDisposable
            =>
            source.ToDisposableSubject("sut");

        /// <summary>
        /// Creates a new <see cref="DisposableSubject{T}"/> instance that wraps the <paramref name="source"/> with the <c>"result"</c> provider name.
        /// </summary>
        /// <typeparam name="T">The type of the source.</typeparam>
        /// <param name="source">The instance to wrap with <see cref="DisposableSubject{T}"/>.</param>
        /// <returns>A new <see cref="DisposableSubject{T}"/> instance.</returns>
        public static DisposableSubject<T> ToResultDisposableSubject<T>(this T source)
            where T : IDisposable
            =>
            source.ToDisposableSubject("result");

        /// <summary>
        /// Creates a new <see cref="DisposableSubject{T}"/> instance that wraps the <paramref name="source"/> with the default provider name.
        /// </summary>
        /// <typeparam name="T">The type of the source.</typeparam>
        /// <param name="source">The instance to wrap with <see cref="DisposableSubject{T}"/>.</param>
        /// <returns>A new <see cref="DisposableSubject{T}"/> instance.</returns>
        public static DisposableSubject<T> ToDisposableSubject<T>(this T source)
            where T : IDisposable
            =>
            new DisposableSubject<T>(source);

        /// <summary>
        /// Creates a new <see cref="DisposableSubject{T}"/> instance that wraps the <paramref name="source"/> with the specified <paramref name="providerName"/>.
        /// </summary>
        /// <typeparam name="T">The type of the source.</typeparam>
        /// <param name="source">The instance to wrap with <see cref="DisposableSubject{T}"/>.</param>
        /// <param name="providerName">Name of the provider.</param>
        /// <returns>A new <see cref="DisposableSubject{T}"/> instance.</returns>
        public static DisposableSubject<T> ToDisposableSubject<T>(this T source, string providerName)
            where T : IDisposable
            =>
            new DisposableSubject<T>(source, providerName);
    }
}
