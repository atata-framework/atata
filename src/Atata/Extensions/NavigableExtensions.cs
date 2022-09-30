namespace Atata
{
    public static class NavigableExtensions
    {
        public static TNavigateTo ClickAndGo<TNavigateTo, TOwner>(this ClickableDelegate<TNavigateTo, TOwner> clickableDelegate)
            where TNavigateTo : PageObject<TNavigateTo>
            where TOwner : PageObject<TOwner>
            =>
            clickableDelegate.GetControl().ClickAndGo();

        public static TNavigateTo ClickAndGo<TNavigateTo, TOwner>(this ButtonDelegate<TNavigateTo, TOwner> buttonDelegate)
            where TNavigateTo : PageObject<TNavigateTo>
            where TOwner : PageObject<TOwner>
            =>
            buttonDelegate.GetControl().ClickAndGo();

        public static TNavigateTo ClickAndGo<TNavigateTo, TOwner>(this LinkDelegate<TNavigateTo, TOwner> linkDelegate)
            where TNavigateTo : PageObject<TNavigateTo>
            where TOwner : PageObject<TOwner>
            =>
            linkDelegate.GetControl().ClickAndGo();
    }
}
