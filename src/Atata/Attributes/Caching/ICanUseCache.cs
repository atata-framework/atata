namespace Atata
{
    /// <summary>
    /// Provides a property that can enable/disable caching.
    /// </summary>
    public interface ICanUseCache
    {
        /// <summary>
        /// Gets a value indicating whether to use cache.
        /// </summary>
        bool UseCache { get; }
    }
}
