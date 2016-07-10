using System;

namespace Atata
{
    public static class IControlVerificationProviderExtensions
    {
        public static TOwner Satisfy<TControl, TOwner>(this IControlVerificationProvider<TControl, TOwner> should, Predicate<TControl> predicate, string message)
            where TControl : IUIComponent<TOwner>
            where TOwner : PageObject<TOwner>
        {
            should.CheckNotNull(nameof(should));
            predicate.CheckNotNull(nameof(predicate));

            return should.Owner;
        }

        public static TOwner Exist<TControl, TOwner>(this IControlVerificationProvider<TControl, TOwner> should)
            where TControl : IUIComponent<TOwner>
            where TOwner : PageObject<TOwner>
        {
            return should.Satisfy(control => control.Exists(new SearchOptions { IsSafely = true, Timeout = TimeSpan.Zero }), "exist");
        }

        public static TOwner BeEnabled<TControl, TOwner>(this IControlVerificationProvider<TControl, TOwner> should)
            where TControl : Control<TOwner>
            where TOwner : PageObject<TOwner>
        {
            return should.Satisfy(control => control.IsEnabled(), "be enabled");
        }
    }
}
