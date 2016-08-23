namespace Atata
{
    public static class Link2Extensions
    {
        public static LinkControl<TNavigateTo, TOwner> GetControl<TNavigateTo, TOwner>(this Link<TNavigateTo, TOwner> clickable)
            where TOwner : PageObject<TOwner>
            where TNavigateTo : PageObject<TNavigateTo>
        {
            return (LinkControl<TNavigateTo, TOwner>)UIComponentResolver.GetControlByDelegate<TOwner>(clickable);
        }

        public static TOwner Click<TNavigateTo, TOwner>(this Link<TNavigateTo, TOwner> clickable)
            where TOwner : PageObject<TOwner>
            where TNavigateTo : PageObject<TNavigateTo>
        {
            return clickable.GetControl().Click();
        }

        public static bool IsEnabled<TNavigateTo, TOwner>(this Link<TNavigateTo, TOwner> clickable)
            where TOwner : PageObject<TOwner>
            where TNavigateTo : PageObject<TNavigateTo>
        {
            return clickable.GetControl().IsEnabled.Value;
        }

        public static bool Exists<TNavigateTo, TOwner>(this Link<TNavigateTo, TOwner> clickable, SearchOptions options = null)
            where TOwner : PageObject<TOwner>
            where TNavigateTo : PageObject<TNavigateTo>
        {
            return clickable.GetControl().Exists(options);
        }

        public static bool Missing<TNavigateTo, TOwner>(this Link<TNavigateTo, TOwner> clickable, SearchOptions options = null)
            where TOwner : PageObject<TOwner>
            where TNavigateTo : PageObject<TNavigateTo>
        {
            return clickable.GetControl().Missing(options);
        }

        public static DataProvider<string, TOwner> Content<TNavigateTo, TOwner>(this Link<TNavigateTo, TOwner> clickable)
            where TOwner : PageObject<TOwner>
            where TNavigateTo : PageObject<TNavigateTo>
        {
            return clickable.GetControl().Content;
        }

        public static UIComponentVerificationProvider<Control<TOwner>, TOwner> Should<TNavigateTo, TOwner>(this Link<TNavigateTo, TOwner> clickable)
            where TOwner : PageObject<TOwner>
            where TNavigateTo : PageObject<TNavigateTo>
        {
            return clickable.GetControl().Should;
        }
    }
}
