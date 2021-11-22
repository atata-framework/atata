using OpenQA.Selenium;

namespace Atata
{
    // TODO: Review to move these methods to UIComponent/UIComponent`1 class.
    public static class ScopeSourceExtensions
    {
        public static ISearchContext GetScopeContext<TOwner>(this ScopeSource scopeSource, IUIComponent<TOwner> component, SearchOptions options = null)
            where TOwner : PageObject<TOwner>
            =>
            GetScopeContext(scopeSource, (UIComponent)component, options);

        public static ISearchContext GetScopeContext(this ScopeSource scopeSource, UIComponent component, SearchOptions options = null)
        {
            options = options ?? new SearchOptions();

            switch (scopeSource)
            {
                case ScopeSource.Parent:
                    return (component.Parent ?? throw UIComponentNotFoundException.ForParentOf(component.ComponentFullName)).GetScopeContext(options);
                case ScopeSource.Grandparent:
                    return (component.Parent?.Parent ?? throw UIComponentNotFoundException.ForGrandparentOf(component.ComponentFullName)).GetScopeContext(options);
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
                    return (parentComponent.Parent ?? throw UIComponentNotFoundException.ForParentOf(parentComponent.ComponentFullName)).ScopeContext;
                case ScopeSource.PageObject:
                    return parentComponent.Owner.ScopeContext;
                case ScopeSource.Page:
                    return (parentComponent?.Context ?? AtataContext.Current).Driver;
                default:
                    throw ExceptionFactory.CreateForUnsupportedEnumValue(scopeSource, nameof(scopeSource));
            }
        }
    }
}
