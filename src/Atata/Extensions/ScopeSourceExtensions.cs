using System;
using OpenQA.Selenium;

namespace Atata
{
    // TODO: Review to move these methods to UIComponent/UIComponent`1 class.
    public static class ScopeSourceExtensions
    {
        [Obsolete("Use GetScopeContext(...) instead.")] // Obsolete since v1.5.0.
        public static IWebElement GetScopeElement<TOwner>(this ScopeSource scopeSource, IUIComponent<TOwner> component, SearchOptions options = null)
            where TOwner : PageObject<TOwner>
        {
            return GetScopeElement(scopeSource, (UIComponent)component, options);
        }

        [Obsolete("Use GetScopeContext(...) instead.")] // Obsolete since v1.5.0.
        public static IWebElement GetScopeElement(this ScopeSource scopeSource, UIComponent component, SearchOptions options = null)
        {
            options = options ?? new SearchOptions();

            switch (scopeSource)
            {
                case ScopeSource.Parent:
                    return component.Parent.GetScope(options);
                case ScopeSource.Grandparent:
                    return component.Parent.Parent.GetScope(options);
                case ScopeSource.PageObject:
                    return component.Owner.GetScope(options);
                case ScopeSource.Page:
                    return component.Driver.GetWithLogging(By.TagName("body").With(options));
                default:
                    throw ExceptionFactory.CreateForUnsupportedEnumValue(scopeSource, nameof(scopeSource));
            }
        }

        [Obsolete("Use GetScopeContextUsingParent(...) instead.")] // Obsolete since v1.5.0.
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
                    return AtataContext.Current.Driver.GetWithLogging(By.TagName("body"));
                default:
                    throw ExceptionFactory.CreateForUnsupportedEnumValue(scopeSource, nameof(scopeSource));
            }
        }

        public static ISearchContext GetScopeContext<TOwner>(this ScopeSource scopeSource, IUIComponent<TOwner> component, SearchOptions options = null)
            where TOwner : PageObject<TOwner>
        {
            return GetScopeContext(scopeSource, (UIComponent)component, options);
        }

        public static ISearchContext GetScopeContext(this ScopeSource scopeSource, UIComponent component, SearchOptions options = null)
        {
            options = options ?? new SearchOptions();

            switch (scopeSource)
            {
                case ScopeSource.Parent:
                    return component.Parent.GetScopeContext(options);
                case ScopeSource.Grandparent:
                    return component.Parent.Parent.GetScopeContext(options);
                case ScopeSource.PageObject:
                    return component.Owner.GetScopeContext(options);
                case ScopeSource.Page:
                    return component.Driver;
                default:
                    throw ExceptionFactory.CreateForUnsupportedEnumValue(scopeSource, nameof(scopeSource));
            }
        }

        public static ISearchContext GetScopeContextUsingParent<TOwner>(this ScopeSource scopeSource, IUIComponent<TOwner> parentComponent)
            where TOwner : PageObject<TOwner>
        {
            switch (scopeSource)
            {
                case ScopeSource.Parent:
                    return parentComponent.ScopeContext;
                case ScopeSource.Grandparent:
                    return parentComponent.Parent.ScopeContext;
                case ScopeSource.PageObject:
                    return parentComponent.Owner.ScopeContext;
                case ScopeSource.Page:
                    return AtataContext.Current.Driver;
                default:
                    throw ExceptionFactory.CreateForUnsupportedEnumValue(scopeSource, nameof(scopeSource));
            }
        }
    }
}
