namespace Atata
{
    /// <summary>
    /// Represents the interface of the object source.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    public interface IObjectSource<out T>
    {
        /// <summary>
        /// Gets the object value/instance.
        /// </summary>
        T Object { get; }

        /// <summary>
        /// Gets the name of the source provider.
        /// </summary>
        string SourceProviderName { get; }

        /// <summary>
        /// Gets a value indicating whether the source is dynamic (value can vary for every value request).
        /// </summary>
        bool IsDynamic { get; }
    }
}
