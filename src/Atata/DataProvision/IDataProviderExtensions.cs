namespace Atata
{
    public static class IDataProviderExtensions
    {
        /// <summary>
        /// Gets the value and records it to <paramref name="value"/> parameter.
        /// </summary>
        /// <typeparam name="TValue">The type of the data value.</typeparam>
        /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
        /// <param name="provider">The data provider.</param>
        /// <param name="value">The value.</param>
        /// <returns>The instance of the owner page object.</returns>
        public static TOwner Get<TValue, TOwner>(this IDataProvider<TValue, TOwner> provider, out TValue value)
        {
            value = provider.Value;
            return provider.Owner;
        }

        /// <summary>
        /// Gets the value and records it to <paramref name="value"/> parameter.
        /// </summary>
        /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
        /// <param name="provider">The data provider.</param>
        /// <param name="value">The value.</param>
        /// <returns>The instance of the owner page object.</returns>
        public static TOwner Get<TOwner>(this IDataProvider<decimal?, TOwner> provider, out int? value)
        {
            value = (int?)provider.Value;
            return provider.Owner;
        }
    }
}
