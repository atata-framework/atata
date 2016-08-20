namespace Atata
{
    public static class INavigableExtensions
    {
        public static TNavigateTo ClickAndGo<TNavigateTo, TOwner>(this INavigable<TNavigateTo, TOwner> navigableControl)
            where TNavigateTo : PageObject<TNavigateTo>
            where TOwner : PageObject<TOwner>
        {
            navigableControl.Click();

            bool temporarily = navigableControl.Metadata.
                GetFirstOrDefaultDeclaringOrComponentAttribute<GoTemporarilyAttribute>()?.
                IsTemporarily ?? false;

            return Go.To<TNavigateTo>(navigate: false, temporarily: temporarily);
        }
    }
}
