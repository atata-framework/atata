using OpenQA.Selenium;

namespace Atata
{
    public class TableRowBase<TOwner> : Control<TOwner>
        where TOwner : PageObject<TOwner>
    {
        protected internal TableSettingsAttribute Settings { get; set; }

        protected IWebElement GetCell(int index)
        {
            return Scope.Get(By.XPath(".//td[{0}]").TableColumn().FormatWith(index + 1));
        }
    }
}
