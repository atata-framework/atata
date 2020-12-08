using OpenQA.Selenium;

namespace Atata
{
    /// <summary>
    /// Represents an item find strategy that finds the item by relative element content using its XPath.
    /// </summary>
    public class FindItemByRelativeElementContentStrategy : TermItemElementFindStrategy
    {
        public FindItemByRelativeElementContentStrategy(string relativeElementXPath)
        {
            RelativeElementXPath = relativeElementXPath;
        }

        public string RelativeElementXPath { get; }

        public override string GetXPathCondition(object parameter, TermOptions termOptions)
        {
            return $"[{RelativeElementXPath}[{TermResolver.CreateXPathCondition(parameter, termOptions)}]]";
        }

        protected override string GetParameterAsString(IWebElement element)
        {
            return element.GetWithLogging(By.XPath(RelativeElementXPath).AtOnce()).Text;
        }
    }
}
