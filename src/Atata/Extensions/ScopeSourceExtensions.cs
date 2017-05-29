using OpenQA.Selenium;

namespace Atata
{
    public static class ScopeSourceExtensions
    {
        public static IWebElement GetScopeElement<TOwner>(this ScopeSource scopeSource, IUIComponent<TOwner> component)
            where TOwner : PageObject<TOwner>
        {
            return GetScopeElement(scopeSource, (UIComponent)component);
        }

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

        public static IWebElement GetScopeElementUsingParent<TOwner>(this ScopeSource scopeSource, IUIComponent<TOwner> parentComponent)
            where TOwner : PageObject<TOwner>
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
                    return AtataContext.Current.Driver.Get(By.TagName("body"));
                default:
                    throw ExceptionFactory.CreateForUnsupportedEnumValue(scopeSource, nameof(scopeSource));
            }
        }
    }
}
