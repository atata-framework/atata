using OpenQA.Selenium;

namespace Atata
{
    public class TableRow<TNavigateTo, TOwner> : TableRowBase<TOwner>
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
