namespace Atata
{
    public static class IControlVerificationProviderExtensions
    {
        public static TOwner Exist<TControl, TOwner>(this IControlVerificationProvider<TControl, TOwner> should)
            where TControl : IUIComponent<TOwner>
            where TOwner : PageObject<TOwner>
        {
            var scope = should.Control.Scope;
            return should.Owner;
        }
    }
}
