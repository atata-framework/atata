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

        public static bool IsEnabled<TOwner>(this Link<TOwner> clickable)
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().IsEnabled.Value;
        }

        public static bool Exists<TOwner>(this Link<TOwner> clickable, SearchOptions options = null)
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().Exists(options);
        }

        public static bool Missing<TOwner>(this Link<TOwner> clickable, SearchOptions options = null)
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().Missing(options);
        }

        public static DataProvider<string, TOwner> Content<TOwner>(this Link<TOwner> clickable)
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().Content;
        }

        public static UIComponentVerificationProvider<Control<TOwner>, TOwner> Should<TOwner>(this Link<TOwner> clickable)
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().Should;
        }
    }
}
