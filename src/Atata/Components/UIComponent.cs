namespace Atata;

/// <summary>
/// Represents the base class for UI components.
/// </summary>
public abstract class UIComponent
{
    internal const string SubComponentSeparator = " / ";

    /// <summary>
    /// Gets the associated session.
    /// </summary>
    public abstract WebDriverSession Session { get; }

    [Obsolete("Instead use either Session.Context or corresponding member of Session.")] // Obsolete since v4.0.0.
    public AtataContext Context => Session.Context;

    /// <inheritdoc cref="IUIComponent{TOwner}.Owner"/>
    public UIComponent Owner { get; internal set; } = null!;

    /// <inheritdoc cref="IUIComponent{TOwner}.Parent"/>
    public UIComponent? Parent { get; internal set; }

    protected internal ILogManager Log => Session.Log;

    protected internal IWebDriver Driver => Session.Driver;

    /// <inheritdoc cref="IUIComponent{TOwner}.ScopeSource"/>
    public abstract ScopeSource ScopeSource { get; }

    protected internal IScopeLocator ScopeLocator { get; internal set; } = null!;

    /// <summary>
    /// Gets a value indicating whether to use scope cache.
    /// Returns a <see cref="ICanUseCache.UsesCache"/> value of an associated with the component
    /// <see cref="UsesScopeCacheAttribute"/> or <see cref="UsesCacheAttribute"/>.
    /// Returns <see langword="false"/>, as by default, when the attribute is not associated.
    /// </summary>
    protected bool UsesScopeCache =>
        Metadata.Get<ICanUseCache>(filter => filter.Where(x => x is UsesCacheAttribute or UsesScopeCacheAttribute))
            ?.UsesCache ?? false;

    /// <inheritdoc cref="IUIComponent{TOwner}.ComponentName"/>
    public string ComponentName { get; set; } = null!;

    internal bool IncludeComponentTypeNameInFullName { get; set; } = true;

    /// <inheritdoc cref="IUIComponent{TOwner}.ComponentTypeName"/>
    public string ComponentTypeName { get; internal set; } = null!;

    /// <inheritdoc cref="IUIComponent{TOwner}.ComponentFullName"/>
    public string ComponentFullName =>
        BuildComponentFullName();

    /// <inheritdoc cref="IUIComponent{TOwner}.Metadata"/>
    public UIComponentMetadata Metadata { get; internal set; } = null!;

    /// <inheritdoc cref="IUIComponent{TOwner}.Scope"/>
    public IWebElement Scope =>
        GetScopeElement();

    /// <summary>
    /// Gets or sets the cached scope element.
    /// </summary>
    protected IWebElement? CachedScope { get; set; }

    /// <inheritdoc cref="IUIComponent{TOwner}.ScopeContext"/>
    public ISearchContext ScopeContext =>
        OnGetScopeContext(new SearchOptions());

    /// <summary>
    /// Gets the <see cref="IWebElement"/> instance that represents the scope HTML element.
    /// Also executes <see cref="TriggerEvents.BeforeAccess" /> and <see cref="TriggerEvents.AfterAccess" /> triggers.
    /// </summary>
    /// <param name="options">
    /// The search options.
    /// If set to <see langword="null"/>, then it uses <c>SearchOptions.Safely()</c>.</param>
    /// <returns>The <see cref="IWebElement"/> instance of the scope.</returns>
    public IWebElement GetScope(SearchOptions? options = null) =>
        GetScopeElement(options ?? SearchOptions.Safely());

    /// <summary>
    /// Gets the <see cref="ISearchContext"/> instance that represents the scope search context
    /// (where to find the children of this component).
    /// Also can execute <see cref="TriggerEvents.BeforeAccess" /> and <see cref="TriggerEvents.AfterAccess" /> triggers.
    /// </summary>
    /// <param name="searchOptions">
    /// The search options.
    /// If set to <see langword="null"/>, then it uses <c>SearchOptions.Safely()</c>.</param>
    /// <returns>The <see cref="ISearchContext"/> instance of the scope context.</returns>
    public ISearchContext GetScopeContext(SearchOptions? searchOptions = null) =>
        OnGetScopeContext(searchOptions ?? SearchOptions.Safely());

    protected virtual ISearchContext OnGetScopeContext(SearchOptions searchOptions) =>
        ShouldUseParentScope()
            ? Parent!.GetScopeContext(searchOptions)
            : GetScopeElement(searchOptions);

    protected IWebElement GetScopeElement(SearchOptions? searchOptions = null)
    {
        if (CachedScope is not null && UsesScopeCache)
            return CachedScope;

        if (ShouldUseParentScope())
            return Parent!.GetScopeElement(searchOptions);

        if (ScopeLocator is null)
            throw new InvalidOperationException($"{nameof(ScopeLocator)} is missing.");

        SearchOptions actualSearchOptions = searchOptions ?? new SearchOptions();

        var cache = Session.UIComponentAccessChainScopeCache;
        bool isActivatedAccessChainCache = cache.AcquireActivation();

        IWebElement? element;

        try
        {
            if (!cache.TryGet(this, actualSearchOptions.Visibility, out element))
            {
                element = isActivatedAccessChainCache
                    ? StaleSafely.Execute(OnGetScopeElement, actualSearchOptions, cache.Clear)
                    : OnGetScopeElement(actualSearchOptions);
            }

            if (!isActivatedAccessChainCache && element is not null)
                cache.Add(this, actualSearchOptions.Visibility, element);
        }
        finally
        {
            if (isActivatedAccessChainCache)
                cache.Release();
        }

        if (UsesScopeCache)
            CachedScope = element;

        return element!;
    }

    internal abstract IWebElement OnGetScopeElement(SearchOptions searchOptions);

    private bool ShouldUseParentScope()
    {
        if (ScopeLocator is not StrategyScopeLocator)
            return false;

        FindAttribute findAttribute = Metadata.ResolveFindAttribute();

        return findAttribute is UseParentScopeAttribute;
    }

    /// <summary>
    /// Builds the full name of the component including parent component full name, own component name and own component type name.
    /// </summary>
    /// <returns>The full name of the component.</returns>
    protected virtual string BuildComponentFullName()
    {
        StringBuilder builder = new StringBuilder();

        if (Parent != null && !Parent.GetType().IsSubclassOfRawGeneric(typeof(PageObject<>)))
        {
            string parentFullName = Parent.ComponentFullName;

            builder
                .Append(parentFullName)
                .Append(SubComponentSeparator);
        }

        if (IncludeComponentTypeNameInFullName)
            builder.AppendFormat("\"{0}\" {1}", ComponentName, ComponentTypeName ?? "component");
        else
            builder.Append(ComponentName);

        return builder.ToString();
    }

    /// <summary>
    /// Determines whether the component exists.
    /// </summary>
    /// <param name="options">
    /// The search options.
    /// If set to <see langword="null"/>, then it uses <c>SearchOptions.Safely()</c>.</param>
    /// <returns><see langword="true"/> if the component exists; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="ElementNotFoundException">
    /// The <paramref name="options"/> has <see cref="SearchOptions.IsSafely"/> property
    /// equal to <see langword="false"/> value and the component doesn't exist.
    /// </exception>
    public bool Exists(SearchOptions? options = null) =>
        GetScopeElement(options ?? SearchOptions.Safely()) != null;

    /// <summary>
    /// Determines whether the component is missing.
    /// </summary>
    /// <param name="options">
    /// The search options.
    /// If set to <see langword="null"/>, then it uses <c>SearchOptions.Safely()</c>.</param>
    /// <returns><see langword="true"/> if the component is missing; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="ElementNotMissingException">
    /// The <paramref name="options"/> has <see cref="SearchOptions.IsSafely"/> property
    /// equal to <see langword="false"/> value and the component exists.
    /// </exception>
    public bool Missing(SearchOptions? options = null) =>
        OnMissing(options ?? SearchOptions.Safely());

    internal virtual bool OnMissing(SearchOptions options) =>
        ScopeLocator.IsMissing(options);

    /// <summary>
    /// Returns a <see cref="string"/> that represents this instance including <see cref="ComponentFullName"/> and <see cref="Scope"/> element details.
    /// </summary>
    /// <returns>
    /// A <see cref="string"/> that represents this instance.
    /// </returns>
    public override string ToString()
    {
        StringBuilder builder = new StringBuilder(ComponentFullName);
        IWebElement scope = GetScopeElement(SearchOptions.SafelyAtOnce());

        if (scope != null)
            builder.AppendLine().Append(scope.ToDetailedString());

        return builder.ToString();
    }

    /// <summary>
    /// Cleans up the current instance.
    /// </summary>
    protected internal abstract void CleanUp();

    /// <inheritdoc cref="IUIComponent{TOwner}.ExecuteBehavior{TBehaviorAttribute}(Action{TBehaviorAttribute})"/>
    public void ExecuteBehavior<TBehaviorAttribute>(Action<TBehaviorAttribute> behaviorExecutionAction)
        where TBehaviorAttribute : MulticastAttribute
    {
        Guard.ThrowIfNull(behaviorExecutionAction);

        var behavior = GetAttributeOrThrow<TBehaviorAttribute>();

        Log.ExecuteSection(
            new ExecuteBehaviorLogSection(this, behavior),
            () => StaleSafely.Execute(
                _ => behaviorExecutionAction.Invoke(behavior),
                Session.ElementFindTimeout));
    }

    /// <inheritdoc cref="IUIComponent{TOwner}.ExecuteBehavior{TBehaviorAttribute}(Action{TBehaviorAttribute})"/>
    public TResult ExecuteBehavior<TBehaviorAttribute, TResult>(Func<TBehaviorAttribute, TResult> behaviorExecutionFunction)
        where TBehaviorAttribute : MulticastAttribute
    {
        Guard.ThrowIfNull(behaviorExecutionFunction);

        var behavior = GetAttributeOrThrow<TBehaviorAttribute>();

        return Log.ExecuteSection(
            new ExecuteBehaviorLogSection(this, behavior),
            () => StaleSafely.Execute(
                _ => behaviorExecutionFunction.Invoke(behavior),
                Session.ElementFindTimeout));
    }

    protected TAttribute GetAttributeOrThrow<TAttribute>()
        where TAttribute : notnull
        =>
        Metadata.TryGet<TAttribute>(out var attribute)
            ? attribute
            : throw AttributeNotFoundException.Create(typeof(TAttribute), ComponentFullName);
}
