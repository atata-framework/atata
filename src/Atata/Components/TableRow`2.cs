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

            IWebElement cellElement = GetCell(Settings.ColumnIndexToClick);
            cellElement.Click();

            Log.EndSection();
            ExecuteTriggers(TriggerEvents.AfterClick);

            return typeof(TOwner) == typeof(TNavigateTo) ? (TNavigateTo)(object)Owner : Owner.GoTo<TNavigateTo>();
        }
    }
}
