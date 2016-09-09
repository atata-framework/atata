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

        protected override By CreateScopeBy()
        {
            StringBuilder xPathBuilder = new StringBuilder(
                "//div[contains(concat(' ', normalize-space(@class), ' '), ' k-window ')]");

            if (CanFindByWindowTitle)
            {
                xPathBuilder.AppendFormat(
                    "[.//*[contains(concat(' ', normalize-space(@class), ' '), ' k-window-title ')][{0}]]",
                    WindowTitleMatch.CreateXPathCondition(WindowTitleValues));
            }

            return By.XPath(xPathBuilder.ToString()).PopupWindow(TermResolver.ToDisplayString(WindowTitleValues));
        }
    }
}
