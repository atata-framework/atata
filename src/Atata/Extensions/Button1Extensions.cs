namespace Atata
{
    public static class Button1Extensions
    {
        public static ButtonControl<TOwner> GetControl<TOwner>(this Button<TOwner> clickable)
            where TOwner : PageObject<TOwner>
        {
            return (ButtonControl<TOwner>)UIComponentResolver.GetControlByDelegate<TOwner>(clickable);
        }

        public static TOwner Click<TOwner>(this Button<TOwner> clickable)
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().Click();
        }

        public static bool IsEnabled<TOwner>(this Button<TOwner> clickable)
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().IsEnabled.Value;
        }

        public static bool Exists<TOwner>(this Button<TOwner> clickable, SearchOptions options = null)
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().Exists(options);
        }

        public static bool Missing<TOwner>(this Button<TOwner> clickable, SearchOptions options = null)
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().Missing(options);
        }

        public static DataProvider<string, TOwner> Content<TOwner>(this Button<TOwner> clickable)
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().Content;
        }

        public static UIComponentVerificationProvider<Control<TOwner>, TOwner> Should<TOwner>(this Button<TOwner> clickable)
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().Should;
        }
    }
}
