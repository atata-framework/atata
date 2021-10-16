namespace Atata
{
    /// <summary>
    /// Indicates that the scope cache of the component should be used.
    /// Caches a scope element of a component when it is requested at first time,
    /// then returns the cached element instance on further scope requests.
    /// </summary>
    public class UseScopeCacheAttribute : MulticastAttribute
    {
        public UseScopeCacheAttribute()
            : this(true)
        {
        }

        public UseScopeCacheAttribute(bool useCache)
        {
            UseCache = useCache;
        }

        /// <summary>
        /// Gets a value indicating whether to use cache.
        /// </summary>
        public bool UseCache { get; }
    }
}
