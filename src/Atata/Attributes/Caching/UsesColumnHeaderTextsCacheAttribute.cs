namespace Atata
{
    /// <summary>
    /// Specifies whether the column header texts cache of the
    /// <see cref="Table{THeader, TRow, TOwner}"/> control should be used.
    /// Caches a value when it is requested at first time,
    /// then returns the cached value on further requests.
    /// </summary>
    public class UsesColumnHeaderTextsCacheAttribute : MulticastAttribute, ICanUseCache
    {
        public UsesColumnHeaderTextsCacheAttribute()
            : this(true)
        {
        }

        public UsesColumnHeaderTextsCacheAttribute(bool useCache) =>
            UsesCache = useCache;

        /// <inheritdoc/>
        public bool UsesCache { get; }
    }
}
