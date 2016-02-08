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

        public IWebElement GetElement(SearchOptions searchOptions = null, string extraXPath = null)
        {
            return element;
        }
    }
}
