using System.Text;
using OpenQA.Selenium;

namespace Atata.KendoUI
{
    public abstract class KendoWindow<T> : PopupWindow<T>
        where T : KendoWindow<T>
    {
        protected KendoWindow(params string[] windowTitleValues)
            : base(windowTitleValues)
        {
        }

        protected override IWebElement GetScope(SearchOptions options)
        {
            StringBuilder xPathBuilder = new StringBuilder(
                ".//div[contains(concat(' ', normalize-space(@class), ' '), ' k-window ')]");

            if (CanFindByWindowTitle)
                xPathBuilder.AppendFormat(
                    "[.//*[contains(concat(' ', normalize-space(@class), ' '), ' k-window-title ')][{0}]]",
                    WindowTitleMatch.CreateXPathCondition(WindowTitleValues));

            return Driver.Get(By.XPath(xPathBuilder.ToString()).PopupWindow(TermResolver.ToDisplayString(WindowTitleValues)).With(options));
        }
    }
}
