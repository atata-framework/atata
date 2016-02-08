using OpenQA.Selenium;

namespace Atata
{
    public class SequalComponentScopeLocateResult : ComponentScopeLocateResult
    {
        public SequalComponentScopeLocateResult(IWebElement element, IComponentScopeLocateStrategy locator, ComponentScopeLocateOptions options = null)
        {
        }

        public SequalComponentScopeLocateResult(By by, IComponentScopeLocateStrategy locator, ComponentScopeLocateOptions options = null)
        {
        }

        public IWebElement ScopeSource { get; private set; }
        public By ScopeSourceBy { get; private set; }
        public IComponentScopeLocateStrategy Strategy { get; private set; }
        public ComponentScopeLocateOptions ScopeLocateOptions { get; private set; }
    }
}
