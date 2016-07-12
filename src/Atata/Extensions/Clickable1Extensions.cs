namespace Atata
{
    public static class Clickable1Extensions
    {
        public static ClickableControl<TOwner> GetControl<TOwner>(this Clickable<TOwner> clickable)
            where TOwner : PageObject<TOwner>
        {
            return (ClickableControl<TOwner>)UIComponentResolver.GetControlByDelegate<TOwner>(clickable);
        }

        public static TOwner Click<TOwner>(this Clickable<TOwner> clickable)
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().Click();
        }

        public static TOwner VerifyEnabled<TOwner>(this Clickable<TOwner> clickable)
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().VerifyEnabled();
        }

        public static TOwner VerifyDisabled<TOwner>(this Clickable<TOwner> clickable)
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().VerifyDisabled();
        }

        public static bool IsEnabled<TOwner>(this Clickable<TOwner> clickable)
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().IsEnabled.Get();
        }

        public static TOwner VerifyExists<TOwner>(this Clickable<TOwner> clickable)
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().VerifyExists();
        }

        public static TOwner VerifyMissing<TOwner>(this Clickable<TOwner> clickable)
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().VerifyMissing();
        }

        public static bool Exists<TOwner>(this Clickable<TOwner> clickable)
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().Exists();
        }

        public static DataProvider<string, TOwner> Content<TOwner>(this Clickable<TOwner> clickable)
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().Content;
        }
    }
}
