using Humanizer;
using OpenQA.Selenium;

namespace Atata
{
    public class FindByLabelStrategy : IElementFindStrategy
    {
        private readonly FindByIdStrategy findByIdStrategy = new FindByIdStrategy();

        public ElementLocator Find(IWebElement scope, ElementFindOptions options)
        {
            string labelCondition = options.GetQualifiersXPathCondition();
            IWebElement label = scope.Get(
                By.XPath(".//label[{0}]{1}".FormatWith(labelCondition, options.GetPositionWrappedXPathCondition())).
                    Label(options.GetQualifiersAsString()).
                    Safely(options.IsSafely));

            if (label == null && options.IsSafely)
                return null;

            string elementId = label.GetAttribute("for");

            ElementFindOptions idOptions = options.Clone();
            idOptions.Qualifiers = new[] { elementId };
            idOptions.Index = null;
            idOptions.Match = QualifierMatch.Equals;

            return findByIdStrategy.Find(scope, idOptions);
        }
    }
}
