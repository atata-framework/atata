namespace Atata
{
    /// <summary>
    /// Specifies whether the component scope cache mechanic should be used.
    /// Caches a scope element of a component when it is requested at first time,
    /// then returns the cached element instance on further scope requests.
    /// </summary>
    public class UsesScopeCacheAttribute : MulticastAttribute, ICanUseCache
    {
        public UsesScopeCacheAttribute()
            : this(true)
        {
        }

        public UsesScopeCacheAttribute(bool useCache)
        {
            UseCache = useCache;
        }

        /// <inheritdoc/>
        public bool UseCache { get; }
    }
}
