namespace Atata
{
    /// <summary>
    /// Specifies whether the column header texts cache of the
    /// <see cref="Table{THeader, TRow, TOwner}"/> control should be used.
    /// Caches a value when it is requested at first time,
    /// then returns the cached value on further requests.
    /// </summary>
    public class UseColumnHeaderTextsCacheAttribute : MulticastAttribute, ICanUseCache
    {
        public UseColumnHeaderTextsCacheAttribute()
            : this(true)
        {
        }

        public UseColumnHeaderTextsCacheAttribute(bool useCache)
        {
            UseCache = useCache;
        }

        /// <inheritdoc/>
        public bool UseCache { get; }
    }
}
