using OpenQA.Selenium;

namespace Atata
{
    public class SequalComponentScopeLocateResult : ComponentScopeLocateResult
    {
        public SequalComponentScopeLocateResult(IWebElement scopeSource, IComponentScopeLocateStrategy strategy, ComponentScopeLocateOptions scopeLocateOptions = null)
        {
            ScopeSource = scopeSource;
            Strategy = strategy;
            ScopeLocateOptions = scopeLocateOptions;
        }

        public SequalComponentScopeLocateResult(By scopeSourceBy, IComponentScopeLocateStrategy strategy, ComponentScopeLocateOptions scopeLocateOptions = null)
        {
            ScopeSourceBy = scopeSourceBy;
            Strategy = strategy;
            ScopeLocateOptions = scopeLocateOptions;
        }

        public IWebElement ScopeSource { get; private set; }
        public By ScopeSourceBy { get; private set; }
        public IComponentScopeLocateStrategy Strategy { get; private set; }
        public ComponentScopeLocateOptions ScopeLocateOptions { get; private set; }
    }
}
