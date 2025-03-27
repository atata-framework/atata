#nullable enable

namespace Atata;

/// <summary>
/// Provides a set of extension methods for <see cref="IHasSourceProviderName"/>.
/// </summary>
public static class IHasSourceProviderNameExtensions
{
    /// <summary>
    /// Sets the name of the source provider.
    /// </summary>
    /// <param name="provider">The provider.</param>
    /// <param name="name">The name.</param>
    public static void SetSourceProviderName(this IHasSourceProviderName provider, string? name) =>
        provider.SourceProviderName = name;
}
