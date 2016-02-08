using OpenQA.Selenium;

namespace Atata
{
    public abstract class ComponentScopeLocateResult
    {
        public IWebElement Get(SearchOptions searchOptions)
        {
            return null;
        }

        public IWebElement Get(SearchOptions searchOptions, string extraXPath)
        {
            return null;
        }
    }
}
