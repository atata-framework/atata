using OpenQA.Selenium;

namespace Atata
{
    public static class ScopeSourceExtensions
    {
        public static IWebElement GetScopeElement(this ScopeSource scopeSource, UIComponent parentComponent)
        {
            switch (scopeSource)
            {
                case ScopeSource.Parent:
                    return parentComponent.Scope;
                case ScopeSource.Grandparent:
                    return parentComponent.Parent.Scope;
                case ScopeSource.PageObject:
                    return parentComponent.Owner.Scope;
                case ScopeSource.Page:
                    return parentComponent.Driver.Get(By.TagName("body"));
                default:
                    throw ExceptionsFactory.CreateForUnsupportedEnumValue(scopeSource, "scopeSource");
            }
        }
    }
}
