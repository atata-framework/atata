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

            if (typeof(TOwner) == typeof(TNavigateTo))
                return (TNavigateTo)(object)Owner;
            else
                return Go.To<TNavigateTo>(navigate: false, temporarily: IsGoTemporarily());
        }

        private bool IsGoTemporarily()
        {
            var attribute = Metadata.GetFirstOrDefaultDeclaringAttribute<GoTemporarilyAttribute>();
            return attribute != null && attribute.IsTemporarily;
        }
    }
}
