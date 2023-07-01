namespace Atata;

// TODO: Review to move these methods to UIComponent/UIComponent`1 class.
public static class ScopeSourceExtensions
{
    public static ISearchContext GetScopeContext<TOwner>(this ScopeSource scopeSource, IUIComponent<TOwner> component, SearchOptions options = null)
        where TOwner : PageObject<TOwner> =>
        GetScopeContext(scopeSource, (UIComponent)component, options);

    public static ISearchContext GetScopeContext(this ScopeSource scopeSource, UIComponent component, SearchOptions options = null)
    {
        options ??= new SearchOptions();

        return scopeSource switch
        {
            ScopeSource.Parent =>
                (component.Parent ?? throw UIComponentNotFoundException.ForParentOf(component.ComponentFullName))
                    .GetScopeContext(options),
            ScopeSource.Grandparent =>
                (component.Parent?.Parent ?? throw UIComponentNotFoundException.ForGrandparentOf(component.ComponentFullName))
                    .GetScopeContext(options),
            ScopeSource.PageObject =>
                component.Owner.GetScopeContext(options),
            ScopeSource.Page =>
                component.Driver,
            _ => throw ExceptionFactory.CreateForUnsupportedEnumValue(scopeSource, nameof(scopeSource))
        };
    }

    public static ISearchContext GetScopeContextUsingParent<TOwner>(this ScopeSource scopeSource, IUIComponent<TOwner> parentComponent)
        where TOwner : PageObject<TOwner> =>
        scopeSource switch
        {
            ScopeSource.Parent =>
                parentComponent.ScopeContext,
            ScopeSource.Grandparent =>
                (parentComponent.Parent ?? throw UIComponentNotFoundException.ForParentOf(parentComponent.ComponentFullName))
                    .ScopeContext,
            ScopeSource.PageObject =>
                parentComponent.Owner.ScopeContext,
            ScopeSource.Page =>
                (parentComponent?.Context ?? AtataContext.Current).Driver,
            _ => throw ExceptionFactory.CreateForUnsupportedEnumValue(scopeSource, nameof(scopeSource))
        };
}
