namespace Atata
{
    public static class Link1Extensions
    {
        public static LinkControl<TOwner> GetControl<TOwner>(this Link<TOwner> clickable)
            where TOwner : PageObject<TOwner>
        {
            return (LinkControl<TOwner>)UIComponentResolver.GetControlByDelegate<TOwner>(clickable);
        }

        public static TOwner Click<TOwner>(this Link<TOwner> clickable)
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().Click();
        }

        public static TOwner VerifyEnabled<TOwner>(this Link<TOwner> clickable)
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().VerifyEnabled();
        }

        public static TOwner VerifyDisabled<TOwner>(this Link<TOwner> clickable)
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().VerifyDisabled();
        }

        public static bool IsEnabled<TOwner>(this Link<TOwner> clickable)
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().IsEnabled.Get();
        }

        public static TOwner VerifyExists<TOwner>(this Link<TOwner> clickable)
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().VerifyExists();
        }

        public static TOwner VerifyMissing<TOwner>(this Link<TOwner> clickable)
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().VerifyMissing();
        }

        public static bool Exists<TOwner>(this Link<TOwner> clickable)
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().Exists();
        }

        public static DataProvider<string, TOwner> Content<TOwner>(this Link<TOwner> clickable)
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().Content;
        }
    }
}
