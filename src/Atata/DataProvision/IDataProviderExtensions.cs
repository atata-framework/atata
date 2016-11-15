namespace Atata
{
    public static class IDataProviderExtensions
    {
        public static TOwner Get<TValue, TOwner>(this IDataProvider<TValue, TOwner> provider, out TValue value)
            where TOwner : PageObject<TOwner>
        {
            value = provider.Get();
            return provider.Owner;
        }

        public static TOwner Get<TOwner>(this IDataProvider<decimal?, TOwner> provider, out int? value)
            where TOwner : PageObject<TOwner>
        {
            value = (int?)provider.Get();
            return provider.Owner;
        }

        internal static string ConvertValueToString<TValue, TOwner>(this IDataProvider<TValue, TOwner> provider, TValue value)
            where TOwner : PageObject<TOwner>
        {
            return TermResolver.ToString(value, provider.ValueTermOptions);
        }
    }
}
