using Humanizer;
using OpenQA.Selenium;
using System;

namespace Atata
{
    public class FindItemByLabelStrategy : IItemElementFindStrategy
    {
        private readonly TermMatch match;

        public FindItemByLabelStrategy(TermMatch match)
        {
            this.match = match;
        }

        public string GetXPathCondition(object parameter)
        {
            string parameterAsString = parameter is Enum ? ((Enum)parameter).ToTitleString() : parameter.ToString();
            return "[ancestor::label[{0}]]".FormatWith(match.CreateXPathCondition(parameterAsString));
        }

        public object GetParameter(IWebElement element)
        {
            return element.Get(By.XPath("ancestor::label").Immediately()).Text;
        }
    }
}
