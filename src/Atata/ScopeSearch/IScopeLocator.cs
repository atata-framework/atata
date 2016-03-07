using OpenQA.Selenium;

namespace Atata
{
    public interface IScopeLocator
    {
        IWebElement GetElement(SearchOptions searchOptions = null, string xPathCondition = null);
        IWebElement[] GetElements(SearchOptions searchOptions = null, string xPathCondition = null);
    }
}
