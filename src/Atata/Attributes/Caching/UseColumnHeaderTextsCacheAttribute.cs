namespace Atata
{
    /// <summary>
    /// Indicates that the column header texts cache of the <see cref="Table{THeader, TRow, TOwner}"/> control should be used.
    /// Caches a value when it is requested at first time,
    /// then returns the cached value on further requests.
    /// </summary>
    public class UseColumnHeaderTextsCacheAttribute : MulticastAttribute
    {
        public UseColumnHeaderTextsCacheAttribute()
            : this(true)
        {
        }

        public UseColumnHeaderTextsCacheAttribute(bool useCache)
        {
            UseCache = useCache;
        }

        /// <summary>
        /// Gets a value indicating whether to use cache.
        /// </summary>
        public bool UseCache { get; }
    }
}
