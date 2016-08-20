namespace Atata.KendoUI
{
    public class KendoGridRow<TNavigateTo, TOwner> : KendoGridRow<TOwner>
        where TNavigateTo : PageObject<TNavigateTo>
        where TOwner : PageObject<TOwner>
    {
        public new TNavigateTo Click()
        {
            base.Click();

            if (typeof(TOwner) == typeof(TNavigateTo))
                return (TNavigateTo)(object)Owner;
            else
                return Go.To<TNavigateTo>(navigate: false, temporarily: GoTemporarily);
        }
    }
}
