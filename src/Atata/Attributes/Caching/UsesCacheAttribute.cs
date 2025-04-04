#nullable enable

namespace Atata;

/// <summary>
/// Specifies whether the component caching mechanics should be used.
/// </summary>
public sealed class UsesCacheAttribute : MulticastAttribute, ICanUseCache
{
    public UsesCacheAttribute()
        : this(true)
    {
    }

    public UsesCacheAttribute(bool useCache) =>
        UsesCache = useCache;

    /// <inheritdoc/>
    public bool UsesCache { get; }
}
