namespace Atata
{
    public static class INavigableExtensions
    {
        public static TNavigateTo ClickAndGo<TNavigateTo, TOwner>(this INavigable<TNavigateTo, TOwner> navigatableControl)
            where TNavigateTo : PageObject<TNavigateTo>
            where TOwner : PageObject<TOwner>
        {
            navigatableControl.Click();
            return Go.To<TNavigateTo>(navigate: false);
        }
    }
}
