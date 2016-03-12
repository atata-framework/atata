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
            string xPathCondition = TermResolver.CreateXPathCondition(parameter, termSettings);
            return "[ancestor::label[{0}]]".FormatWith(xPathCondition);
        }

        public T GetParameter<T>(IWebElement element)
        {
            string value = element.Get(By.XPath("ancestor::label").Immediately()).Text;
            return TermResolver.FromString<T>(value, termSettings);
        }
    }
}
