namespace Atata
{
    public static class IDataVerificationProviderExtensions
    {
        public static TOwner Equal<TData, TOwner>(this IDataVerificationProvider<TData, TOwner> should, TData expectedValue)
            where TOwner : PageObject<TOwner>
        {
            return should.Owner;
        }

        public static TOwner StartWith<TOwner>(this IDataVerificationProvider<string, TOwner> should, string expectedValue)
            where TOwner : PageObject<TOwner>
        {
            return should.Owner;
        }
    }
}
