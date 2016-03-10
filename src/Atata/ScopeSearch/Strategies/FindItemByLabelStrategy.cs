using Humanizer;
using OpenQA.Selenium;

namespace Atata
{
    public class FindItemByLabelStrategy : IItemElementFindStrategy
    {
        private readonly ITermSettings termSettings;

        public FindItemByLabelStrategy(ITermSettings termSettings)
        {
            this.termSettings = termSettings;
        }

        public string GetXPathCondition(object parameter)
        {
            string parameterAsString = TermResolver.ToString(parameter, termSettings);
            TermMatch match = TermResolver.GetMatch(parameter, termSettings);
            return "[ancestor::label[{0}]]".FormatWith(match.CreateXPathCondition(parameterAsString));
        }

        public object GetParameter(IWebElement element)
        {
            return element.Get(By.XPath("ancestor::label").Immediately()).Text;
        }
    }
}
