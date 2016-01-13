using OpenQA.Selenium;

namespace Atata
{
    public class TableRow<TNavigateTo, TOwner> : TableRowBase<TOwner>
        where TNavigateTo : PageObject<TNavigateTo>
        where TOwner : PageObject<TOwner>
    {
        public new TNavigateTo Click()
        {
            Log.StartClickingSection(ComponentName);
            RunTriggersBefore();

            IWebElement cellElement = GetCell(Settings.ColumnIndexToClick);
            cellElement.Click();

            RunTriggersAfter();
            Log.EndSection();

            return typeof(TOwner) == typeof(TNavigateTo) ? (TNavigateTo)(object)Owner : Owner.GoTo<TNavigateTo>();
        }
    }
}
