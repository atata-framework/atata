namespace Atata;

/// <summary>
/// Provides a property that can enable/disable caching.
/// </summary>
public interface ICanUseCache
{
    /// <summary>
    /// Gets a value indicating whether the cache is enabled.
    /// </summary>
    bool UsesCache { get; }
}
