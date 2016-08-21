namespace Atata
{
    public static class Button2Extensions
    {
        public static ButtonControl<TNavigateTo, TOwner> GetControl<TNavigateTo, TOwner>(this Button<TNavigateTo, TOwner> clickable)
            where TOwner : PageObject<TOwner>
            where TNavigateTo : PageObject<TNavigateTo>
        {
            return (ButtonControl<TNavigateTo, TOwner>)UIComponentResolver.GetControlByDelegate<TOwner>(clickable);
        }

        public static TNavigateTo Click<TNavigateTo, TOwner>(this Button<TNavigateTo, TOwner> clickable)
            where TOwner : PageObject<TOwner>
            where TNavigateTo : PageObject<TNavigateTo>
        {
            return clickable.GetControl().Click();
        }

        public static bool IsEnabled<TNavigateTo, TOwner>(this Button<TNavigateTo, TOwner> clickable)
            where TOwner : PageObject<TOwner>
            where TNavigateTo : PageObject<TNavigateTo>
        {
            return clickable.GetControl().IsEnabled.Value;
        }

        public static bool Exists<TNavigateTo, TOwner>(this Button<TNavigateTo, TOwner> clickable, SearchOptions options = null)
            where TOwner : PageObject<TOwner>
            where TNavigateTo : PageObject<TNavigateTo>
        {
            return clickable.GetControl().Exists(options);
        }

        public static bool Missing<TNavigateTo, TOwner>(this Button<TNavigateTo, TOwner> clickable, SearchOptions options = null)
            where TOwner : PageObject<TOwner>
            where TNavigateTo : PageObject<TNavigateTo>
        {
            return clickable.GetControl().Missing(options);
        }

        public static DataProvider<string, TOwner> Content<TNavigateTo, TOwner>(this Button<TNavigateTo, TOwner> clickable)
            where TOwner : PageObject<TOwner>
            where TNavigateTo : PageObject<TNavigateTo>
        {
            return clickable.GetControl().Content;
        }

        public static UIComponentVerificationProvider<Control<TOwner>, TOwner> Should<TNavigateTo, TOwner>(this Button<TNavigateTo, TOwner> clickable)
            where TOwner : PageObject<TOwner>
            where TNavigateTo : PageObject<TNavigateTo>
        {
            return clickable.GetControl().Should;
        }
    }
}
