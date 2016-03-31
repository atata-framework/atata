using OpenQA.Selenium;
using System.Text;

namespace Atata.KendoUI
{
    public abstract class KendoWindow<T> : PopupWindow<T>
        where T : PopupWindow<T>
    {
        protected KendoWindow()
        {
        }

        protected override IWebElement GetScope(SearchOptions options)
        {
            StringBuilder xPathBuilder = new StringBuilder(
                ".//div[contains(concat(' ', normalize-space(@class), ' '), ' k-window ')]");

            if (!string.IsNullOrWhiteSpace(WindowTitle))
                xPathBuilder.AppendFormat(
                    "[div[contains(concat(' ', normalize-space(@class), ' '), ' k-window-titlebar ')][contains(., '{0}')]]",
                    WindowTitle);

            return Driver.Get(By.XPath(xPathBuilder.ToString()).PopupWindow(WindowTitle).With(options));
        }
    }
}
