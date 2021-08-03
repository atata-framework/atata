using OpenQA.Selenium;

namespace Atata
{
    public class DefinedScopeLocator : IScopeLocator
    {
        private readonly IWebElement _element;

        public DefinedScopeLocator(IWebElement element)
        {
            _element = element;
        }

        public IWebElement GetElement(SearchOptions searchOptions = null, string xPathCondition = null)
        {
            return _element;
        }

        public IWebElement[] GetElements(SearchOptions searchOptions = null, string xPathCondition = null)
        {
            return new[] { _element };
        }

        public bool IsMissing(SearchOptions searchOptions = null, string xPathCondition = null)
        {
            return _element == null;
        }
    }
}
