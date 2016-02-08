using OpenQA.Selenium;

namespace Atata
{
    public interface IScopeLocator
    {
        IWebElement GetElement(SearchOptions searchOptions = null, string extraXPath = null);
    }
}
