namespace Atata
{
    /// <summary>
    /// Specifies whether the component value cache mechanic should be used.
    /// Caches a value of a component when it is requested at first time,
    /// then returns the cached value on further scope requests.
    /// </summary>
    public class UsesValueCacheAttribute : MulticastAttribute, ICanUseCache
    {
        public UsesValueCacheAttribute()
            : this(true)
        {
        }

        public UsesValueCacheAttribute(bool useCache) =>
            UsesCache = useCache;

        /// <inheritdoc/>
        public bool UsesCache { get; }
    }
}
