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

        protected override IWebElement GetScope(bool isSafely)
        {
            StringBuilder xPathBuilder = new StringBuilder(
                ".//div[contains(concat(' ', normalize-space(@class), ' '), ' k-window ')]");

            if (!string.IsNullOrWhiteSpace(Title))
                xPathBuilder.AppendFormat(
                    "[div[contains(concat(' ', normalize-space(@class), ' '), ' k-window-titlebar ')][contains(., '{0}')]]",
                    Title);

            return Driver.Get(By.XPath(xPathBuilder.ToString()).PopupWindow(Title).Safely(isSafely));
        }
    }
}
