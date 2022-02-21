namespace Atata
{
    /// <summary>
    /// Represents the static object source that takes an object in constructor and returns it.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    public class StaticObjectSource<T> : IObjectSource<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StaticObjectSource{T}"/> class.
        /// </summary>
        /// <param name="source">The source object.</param>
        public StaticObjectSource(T source)
        {
            Object = source;
        }

        /// <inheritdoc/>
        public T Object { get; }

        /// <inheritdoc/>
        public string SourceProviderName => null;

        /// <inheritdoc/>
        public bool IsDynamic => false;
    }
}
