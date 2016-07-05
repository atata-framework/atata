using System;

namespace Atata
{
    public static class IControlVerificationProviderExtensions
    {
        public static TOwner Satisfy<TControl, TOwner>(this IControlVerificationProvider<TControl, TOwner> should, string message, Predicate<TControl> predicate)
            where TControl : IUIComponent<TOwner>
            where TOwner : PageObject<TOwner>
        {
            return should.Owner;
        }

        public static TOwner Exist<TControl, TOwner>(this IControlVerificationProvider<TControl, TOwner> should)
            where TControl : IUIComponent<TOwner>
            where TOwner : PageObject<TOwner>
        {
            var scope = should.Control.Scope;
            return should.Owner;
        }

        public static TOwner BeEnabled<TControl, TOwner>(this IControlVerificationProvider<TControl, TOwner> should)
            where TControl : Control<TOwner>
            where TOwner : PageObject<TOwner>
        {
            return should.Satisfy("be enabled", x => x.IsEnabled());
        }
    }
}
