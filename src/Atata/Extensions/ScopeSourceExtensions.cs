using OpenQA.Selenium;

namespace Atata
{
    public static class ScopeSourceExtensions
    {
        public static IWebElement GetScopeElement(this ScopeSource scopeSource, UIComponent component)
        {
            switch (scopeSource)
            {
                case ScopeSource.Parent:
                    return component.Parent.Scope;
                case ScopeSource.Grandparent:
                    return component.Parent.Parent.Scope;
                case ScopeSource.PageObject:
                    return component.Owner.Scope;
                case ScopeSource.Page:
                    return component.Driver.Get(By.TagName("body"));
                default:
                    throw ExceptionFactory.CreateForUnsupportedEnumValue(scopeSource, nameof(scopeSource));
            }
        }
    }
}
