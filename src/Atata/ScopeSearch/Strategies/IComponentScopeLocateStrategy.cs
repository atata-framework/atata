using System;
using OpenQA.Selenium;

namespace Atata
{
    [Obsolete("Use IComponentScopeFindStrategy instead.")]
    public interface IComponentScopeLocateStrategy
    {
        ComponentScopeLocateResult Find(IWebElement scope, ComponentScopeLocateOptions options, SearchOptions searchOptions);
    }
}
