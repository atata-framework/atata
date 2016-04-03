namespace Atata
{
    public static class Clickable1Extensions
    {
        public static Control<TOwner> GetControl<TOwner>(this _Clickable<TOwner> clickable)
            where TOwner : PageObject<TOwner>
        {
            return UIComponentResolver.GetControlByDelegate<TOwner>(clickable);
        }

        public static TOwner VerifyEnabled<TOwner>(this _Clickable<TOwner> clickable)
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().VerifyEnabled();
        }

        public static TOwner VerifyDisabled<TOwner>(this _Clickable<TOwner> clickable)
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().VerifyDisabled();
        }

        public static bool IsEnabled<TNavigateTo, TOwner>(this _Clickable<TOwner> clickable)
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().IsEnabled();
        }

        public static TOwner VerifyExists<TOwner>(this _Clickable<TOwner> clickable)
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().VerifyExists();
        }

        public static TOwner VerifyMissing<TOwner>(this _Clickable<TOwner> clickable)
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().VerifyMissing();
        }

        public static bool Exists<TOwner>(this _Clickable<TOwner> clickable)
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().Exists();
        }

        public static TOwner VerifyContent<TOwner>(this _Clickable<TOwner> clickable, string content, TermMatch match = TermMatch.Equals)
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().VerifyContent(content, match);
        }

        public static TOwner VerifyContent<TOwner>(this _Clickable<TOwner> clickable, string[] content, TermMatch match = TermMatch.Equals)
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().VerifyContent(content, match);
        }
    }
}
