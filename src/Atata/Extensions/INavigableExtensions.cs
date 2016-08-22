using System;

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

            Func<object> navigationPageObjectCreator = navigableControl.Metadata.
                GetFirstOrDefaultDeclaringOrComponentAttribute<NavigationPageObjectCreatorAttribute>()?.
                Creator;

            TNavigateTo pageObject = navigationPageObjectCreator != null
                ? (TNavigateTo)navigationPageObjectCreator()
                : null;

            return Go.To(pageObject: pageObject, navigate: false, temporarily: temporarily);
        }
    }
}
