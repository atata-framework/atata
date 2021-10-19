namespace Atata
{
    /// <summary>
    /// Specifies whether the component scope cache mechanic should be used.
    /// Caches a scope element of a component when it is requested at first time,
    /// then returns the cached element instance on further scope requests.
    /// </summary>
    public class UseScopeCacheAttribute : MulticastAttribute, ICanUseCache
    {
        public UseScopeCacheAttribute()
            : this(true)
        {
        }

        public UseScopeCacheAttribute(bool useCache)
        {
            UseCache = useCache;
        }

        /// <inheritdoc/>
        public bool UseCache { get; }
    }
}
