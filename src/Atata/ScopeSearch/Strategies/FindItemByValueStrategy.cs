using Humanizer;
using OpenQA.Selenium;

namespace Atata
{
    public class FindItemByValueStrategy : IItemElementFindStrategy
    {
        private readonly ITermSettings termSettings;

        public FindItemByValueStrategy(ITermSettings termSettings)
        {
            this.termSettings = termSettings;
        }

        public string GetXPathCondition(object parameter)
        {
            string xPathCondition = TermResolver.CreateXPathCondition(parameter, termSettings, "@value");
            return "[{0}]".FormatWith(xPathCondition);
        }

        public T GetParameter<T>(IWebElement element)
        {
            string value = element.GetValue();
            return TermResolver.FromString<T>(value, termSettings);
        }
    }
}
