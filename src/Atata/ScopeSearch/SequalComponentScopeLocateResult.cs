using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;

namespace Atata
{
    public class SequalComponentScopeLocateResult : ComponentScopeLocateResult
    {
        public SequalComponentScopeLocateResult(IWebElement scopeSource, IComponentScopeLocateStrategy strategy, ComponentScopeLocateOptions scopeLocateOptions = null)
            : this(new[] { scopeSource ?? throw new ArgumentNullException(nameof(scopeSource)) }, strategy, scopeLocateOptions)
        {
        }

        public SequalComponentScopeLocateResult(IEnumerable<IWebElement> scopeSources, IComponentScopeLocateStrategy strategy, ComponentScopeLocateOptions scopeLocateOptions = null)
            : this(strategy, scopeLocateOptions)
        {
            ScopeSources = scopeSources ?? throw new ArgumentNullException(nameof(scopeSources));
        }

        public SequalComponentScopeLocateResult(By scopeSourceBy, IComponentScopeLocateStrategy strategy, ComponentScopeLocateOptions scopeLocateOptions = null)
            : this(strategy, scopeLocateOptions)
        {
            ScopeSourceBy = scopeSourceBy ?? throw new ArgumentNullException(nameof(scopeSourceBy));
        }

        private SequalComponentScopeLocateResult(IComponentScopeLocateStrategy strategy, ComponentScopeLocateOptions scopeLocateOptions)
        {
            Strategy = strategy;
            ScopeLocateOptions = scopeLocateOptions;
        }

        [Obsolete("Use ScopeSources instead.")] // Obsolete since v1.1.0.
        public IWebElement ScopeSource => ScopeSources?.FirstOrDefault();

        public IEnumerable<IWebElement> ScopeSources { get; } = Enumerable.Empty<IWebElement>();

        public By ScopeSourceBy { get; }

        public IComponentScopeLocateStrategy Strategy { get; }

        public ComponentScopeLocateOptions ScopeLocateOptions { get; }
    }
}
