namespace Atata
{
    public static class ButtonDelegate2Extensions
    {
        public static Button<TNavigateTo, TOwner> GetControl<TNavigateTo, TOwner>(this ButtonDelegate<TNavigateTo, TOwner> clickable)
            where TNavigateTo : PageObject<TNavigateTo>
            where TOwner : PageObject<TOwner>
        {
            return (Button<TNavigateTo, TOwner>)UIComponentResolver.GetControlByDelegate<TOwner>(clickable);
        }

        public static TOwner Click<TNavigateTo, TOwner>(this ButtonDelegate<TNavigateTo, TOwner> clickable)
            where TNavigateTo : PageObject<TNavigateTo>
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().Click();
        }

        public static bool IsEnabled<TNavigateTo, TOwner>(this ButtonDelegate<TNavigateTo, TOwner> clickable)
            where TNavigateTo : PageObject<TNavigateTo>
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().IsEnabled.Value;
        }

        public static bool Exists<TNavigateTo, TOwner>(this ButtonDelegate<TNavigateTo, TOwner> clickable, SearchOptions options = null)
            where TNavigateTo : PageObject<TNavigateTo>
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().Exists(options);
        }

        public static bool Missing<TNavigateTo, TOwner>(this ButtonDelegate<TNavigateTo, TOwner> clickable, SearchOptions options = null)
            where TNavigateTo : PageObject<TNavigateTo>
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().Missing(options);
        }

        public static DataProvider<string, TOwner> Content<TNavigateTo, TOwner>(this ButtonDelegate<TNavigateTo, TOwner> clickable)
            where TNavigateTo : PageObject<TNavigateTo>
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().Content;
        }

        public static UIComponentVerificationProvider<Control<TOwner>, TOwner> Should<TNavigateTo, TOwner>(this ButtonDelegate<TNavigateTo, TOwner> clickable)
            where TNavigateTo : PageObject<TNavigateTo>
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().Should;
        }
    }
}
