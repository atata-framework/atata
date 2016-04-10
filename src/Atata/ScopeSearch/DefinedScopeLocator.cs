using OpenQA.Selenium;

namespace Atata
{
    public class DefinedScopeLocator : IScopeLocator
    {
        private readonly IWebElement element;

        public DefinedScopeLocator(IWebElement element)
        {
            this.element = element;
        }

        public IWebElement GetElement(SearchOptions searchOptions = null, string xPathCondition = null)
        {
            return element;
        }

        public IWebElement[] GetElements(SearchOptions searchOptions = null, string xPathCondition = null)
        {
            return new[] { element };
        }

        public bool IsMissing(SearchOptions searchOptions = null, string xPathCondition = null)
        {
            return element == null;
        }
    }
}
