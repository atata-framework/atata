using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;

namespace Atata
{
    public class SubsequentComponentScopeFindResult : ComponentScopeLocateResult
    {
        public SubsequentComponentScopeFindResult(ISearchContext scopeSource, IComponentScopeFindStrategy strategy, ComponentScopeLocateOptions scopeLocateOptions = null)
            : this(new[] { scopeSource ?? throw new ArgumentNullException(nameof(scopeSource)) }, strategy, scopeLocateOptions)
        {
        }

        public SubsequentComponentScopeFindResult(IEnumerable<ISearchContext> scopeSources, IComponentScopeFindStrategy strategy, ComponentScopeLocateOptions scopeLocateOptions = null)
            : this(strategy, scopeLocateOptions)
        {
            ScopeSources = scopeSources ?? throw new ArgumentNullException(nameof(scopeSources));
        }

        public SubsequentComponentScopeFindResult(By scopeSourceBy, IComponentScopeFindStrategy strategy, ComponentScopeLocateOptions scopeLocateOptions = null)
            : this(strategy, scopeLocateOptions)
        {
            ScopeSourceBy = scopeSourceBy ?? throw new ArgumentNullException(nameof(scopeSourceBy));
        }

        private SubsequentComponentScopeFindResult(IComponentScopeFindStrategy strategy, ComponentScopeLocateOptions scopeLocateOptions)
        {
            Strategy = strategy;
            ScopeLocateOptions = scopeLocateOptions;
        }

        public IEnumerable<ISearchContext> ScopeSources { get; } = Enumerable.Empty<IWebElement>();

        public By ScopeSourceBy { get; }

        public IComponentScopeFindStrategy Strategy { get; }

        public ComponentScopeLocateOptions ScopeLocateOptions { get; }
    }
}
