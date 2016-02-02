using OpenQA.Selenium;

namespace Atata
{
    public class TableRow<TNavigateTo, TOwner> : TableRowBase<TOwner>
        where TNavigateTo : PageObject<TNavigateTo>
        where TOwner : PageObject<TOwner>
    {
        public new TNavigateTo Click()
        {
            RunTriggers(TriggerEvents.BeforeClick);
            Log.StartClickingSection(ComponentName);

            IWebElement cellElement = GetCell(Settings.ColumnIndexToClick);
            cellElement.Click();

            Log.EndSection();
            RunTriggers(TriggerEvents.AfterClick);

            return typeof(TOwner) == typeof(TNavigateTo) ? (TNavigateTo)(object)Owner : Owner.GoTo<TNavigateTo>();
        }
    }
}
