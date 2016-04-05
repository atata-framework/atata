namespace Atata
{
    public static class Clickable2Extensions
    {
        public static Control<TOwner> GetControl<TNavigateTo, TOwner>(this Clickable<TNavigateTo, TOwner> clickable)
            where TOwner : PageObject<TOwner>
            where TNavigateTo : PageObject<TNavigateTo>
        {
            return UIComponentResolver.GetControlByDelegate<TOwner>(clickable);
        }

        public static TOwner VerifyEnabled<TNavigateTo, TOwner>(this Clickable<TNavigateTo, TOwner> clickable)
            where TOwner : PageObject<TOwner>
            where TNavigateTo : PageObject<TNavigateTo>
        {
            return clickable.GetControl().VerifyEnabled();
        }

        public static TOwner VerifyDisabled<TNavigateTo, TOwner>(this Clickable<TNavigateTo, TOwner> clickable)
            where TOwner : PageObject<TOwner>
            where TNavigateTo : PageObject<TNavigateTo>
        {
            return clickable.GetControl().VerifyDisabled();
        }

        public static bool IsEnabled<TNavigateTo, TOwner>(this Clickable<TNavigateTo, TOwner> clickable)
            where TOwner : PageObject<TOwner>
            where TNavigateTo : PageObject<TNavigateTo>
        {
            return clickable.GetControl().IsEnabled();
        }

        public static TOwner VerifyExists<TNavigateTo, TOwner>(this Clickable<TNavigateTo, TOwner> clickable)
            where TOwner : PageObject<TOwner>
            where TNavigateTo : PageObject<TNavigateTo>
        {
            return clickable.GetControl().VerifyExists();
        }

        public static TOwner VerifyMissing<TNavigateTo, TOwner>(this Clickable<TNavigateTo, TOwner> clickable)
            where TOwner : PageObject<TOwner>
            where TNavigateTo : PageObject<TNavigateTo>
        {
            return clickable.GetControl().VerifyMissing();
        }

        public static bool Exists<TNavigateTo, TOwner>(this Clickable<TNavigateTo, TOwner> clickable)
            where TOwner : PageObject<TOwner>
            where TNavigateTo : PageObject<TNavigateTo>
        {
            return clickable.GetControl().Exists();
        }

        public static TOwner VerifyContent<TNavigateTo, TOwner>(this Clickable<TNavigateTo, TOwner> clickable, string content, TermMatch match = TermMatch.Equals)
            where TOwner : PageObject<TOwner>
            where TNavigateTo : PageObject<TNavigateTo>
        {
            return clickable.GetControl().VerifyContent(content, match);
        }

        public static TOwner VerifyContent<TNavigateTo, TOwner>(this Clickable<TNavigateTo, TOwner> clickable, string[] content, TermMatch match = TermMatch.Equals)
            where TOwner : PageObject<TOwner>
            where TNavigateTo : PageObject<TNavigateTo>
        {
            return clickable.GetControl().VerifyContent(content, match);
        }
    }
}
