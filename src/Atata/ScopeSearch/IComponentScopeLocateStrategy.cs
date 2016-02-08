using OpenQA.Selenium;

namespace Atata
{
    public interface IComponentScopeLocateStrategy
    {
        ComponentScopeLocateResult Find(IWebElement scope, ComponentScopeLocateOptions options, SearchOptions searchOptions);
    }
}
