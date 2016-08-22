namespace Atata
{
    public static class NavigableExtensions
    {
        public static TNavigateTo ClickAndGo<TNavigateTo, TOwner>(this Clickable<TNavigateTo, TOwner> clickable)
            where TNavigateTo : PageObject<TNavigateTo>
            where TOwner : PageObject<TOwner>
        {
            return clickable.GetControl().ClickAndGo();
        }

        public static TNavigateTo ClickAndGo<TNavigateTo, TOwner>(this Button<TNavigateTo, TOwner> button)
            where TNavigateTo : PageObject<TNavigateTo>
            where TOwner : PageObject<TOwner>
        {
            return button.GetControl().ClickAndGo();
        }

        public static TNavigateTo ClickAndGo<TNavigateTo, TOwner>(this Link<TNavigateTo, TOwner> link)
            where TNavigateTo : PageObject<TNavigateTo>
            where TOwner : PageObject<TOwner>
        {
            return link.GetControl().ClickAndGo();
        }
    }
}
