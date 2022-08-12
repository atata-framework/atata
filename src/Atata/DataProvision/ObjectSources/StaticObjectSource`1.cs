namespace Atata
{
    /// <summary>
    /// Represents the static object source that takes an object in constructor and returns it.
    /// </summary>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    public class StaticObjectSource<TObject> : IObjectSource<TObject>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StaticObjectSource{TObject}"/> class.
        /// </summary>
        /// <param name="source">The source object.</param>
        public StaticObjectSource(TObject source) =>
            Object = source;

        /// <inheritdoc/>
        public TObject Object { get; }

        /// <inheritdoc/>
        public string SourceProviderName => null;

        /// <inheritdoc/>
        public bool IsDynamic => false;
    }
}
