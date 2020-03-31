using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;

namespace Atata
{
    [Obsolete("Use SequalComponentScopeFindResult class instead.")] // Obsolete since v1.5.0.
    public class SequalComponentScopeLocateResult : SequalComponentScopeFindResult
    {
        public SequalComponentScopeLocateResult(IWebElement scopeSource, IComponentScopeLocateStrategy strategy, ComponentScopeLocateOptions scopeLocateOptions = null)
            : this(new[] { scopeSource ?? throw new ArgumentNullException(nameof(scopeSource)) }, strategy, scopeLocateOptions)
        {
        }

        public SequalComponentScopeLocateResult(IEnumerable<IWebElement> scopeSources, IComponentScopeLocateStrategy strategy, ComponentScopeLocateOptions scopeLocateOptions = null)
            : base(scopeSources ?? throw new ArgumentNullException(nameof(scopeSources)), null, strategy, scopeLocateOptions)
        {
        }

        public SequalComponentScopeLocateResult(By scopeSourceBy, IComponentScopeLocateStrategy strategy, ComponentScopeLocateOptions scopeLocateOptions = null)
            : base(null, scopeSourceBy ?? throw new ArgumentNullException(nameof(scopeSourceBy)), strategy, scopeLocateOptions)
        {
        }

        [Obsolete("Use ScopeSources instead.")] // Obsolete since v1.1.0.
        public IWebElement ScopeSource => ScopeSources?.FirstOrDefault();

        public new IEnumerable<IWebElement> ScopeSources => base.ScopeSources.Cast<IWebElement>();

        public new IComponentScopeLocateStrategy Strategy => (IComponentScopeLocateStrategy)ActualStrategy;
    }
}
