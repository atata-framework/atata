namespace Atata
{
    /// <summary>
    /// Provides a set of extension methods for <see cref="IHasProviderName"/>.
    /// </summary>
    public static class IHasProviderNameExtensions
    {
        /// <summary>
        /// Sets the name of the provider.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="name">The name.</param>
        public static void SetProviderName(this IHasProviderName provider, string name) =>
            provider.ProviderName = name;
    }
}
