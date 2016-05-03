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

        public static TOwner VerifyEnabled<TOwner>(this Button<TOwner> clickable)
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().VerifyEnabled();
        }

        public static TOwner VerifyDisabled<TOwner>(this Button<TOwner> clickable)
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().VerifyDisabled();
        }

        public static bool IsEnabled<TNavigateTo, TOwner>(this Button<TOwner> clickable)
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().IsEnabled();
        }

        public static TOwner VerifyExists<TOwner>(this Button<TOwner> clickable)
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().VerifyExists();
        }

        public static TOwner VerifyMissing<TOwner>(this Button<TOwner> clickable)
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().VerifyMissing();
        }

        public static bool Exists<TOwner>(this Button<TOwner> clickable)
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().Exists();
        }

        public static IUIComponentValueProvider<string, TOwner> Content<TOwner>(this Button<TOwner> clickable)
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().Content;
        }
    }
}
