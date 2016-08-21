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

        public static bool IsEnabled<TOwner>(this Clickable<TOwner> clickable)
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().IsEnabled.Value;
        }

        public static bool Exists<TOwner>(this Clickable<TOwner> clickable, SearchOptions options = null)
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().Exists(options);
        }

        public static bool Missing<TOwner>(this Clickable<TOwner> clickable, SearchOptions options = null)
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().Missing(options);
        }

        public static DataProvider<string, TOwner> Content<TOwner>(this Clickable<TOwner> clickable)
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().Content;
        }

        public static UIComponentVerificationProvider<Control<TOwner>, TOwner> Should<TOwner>(this Clickable<TOwner> clickable)
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().Should;
        }
    }
}
