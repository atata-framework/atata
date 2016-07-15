using OpenQA.Selenium;

namespace Atata.KendoUI
{
    public class KendoGridRow<TNavigateTo, TOwner> : KendoGridRow<TOwner>
        where TNavigateTo : PageObject<TNavigateTo>
        where TOwner : PageObject<TOwner>
    {
        public new TNavigateTo Click()
        {
            ExecuteTriggers(TriggerEvents.BeforeClick);
            Log.StartClickingSection(ComponentFullName);

            IWebElement elementToClick = ColumnIndexToClick.HasValue ? GetCell(ColumnIndexToClick.Value) : Scope;
            elementToClick.Click();

            Log.EndSection();
            ExecuteTriggers(TriggerEvents.AfterClick);

            if (typeof(TOwner) == typeof(TNavigateTo))
                return (TNavigateTo)(object)Owner;
            else
                return Go.To<TNavigateTo>(navigate: false, temporarily: GoTemporarily);
        }
    }
}
