namespace Atata;

/// <summary>
/// Represents the base class for UI components (page objects and controls).
/// </summary>
/// <typeparam name="TOwner">The type of the owner page object.</typeparam>
[GetsContentFromSource(ContentSource.Text)]
public abstract class UIComponent<TOwner> : UIComponent, IUIComponent<TOwner>
    where TOwner : PageObject<TOwner>
{
    private const string IsInViewportScript = @"
const element = arguments[0];
const rect = element.getBoundingClientRect();

return (
  rect.top >= 0 &&
  rect.left >= 0 &&
  Math.floor(rect.bottom) <= (window.innerHeight || document.documentElement.clientHeight) &&
  Math.floor(rect.right) <= (window.innerWidth || document.documentElement.clientWidth));";

    private readonly List<TriggerEvents> _currentDeniedTriggers = [];

    protected UIComponent()
    {
        Controls = new UIComponentChildrenList<TOwner>(this);
        DomAttributes = new UIComponentDomAttributesProvider<TOwner>(this);
        DomProperties = new UIComponentDomPropertiesProvider<TOwner>(this);
        Css = new UIComponentCssProvider<TOwner>(this);
        Script = new UIComponentScriptExecutor<TOwner>(this);
    }

    /// <inheritdoc cref="IUIComponent{TOwner}.Owner"/>
    public new TOwner Owner
    {
        get => (TOwner)base.Owner;
        internal set => base.Owner = value;
    }

    /// <inheritdoc cref="IUIComponent{TOwner}.Parent"/>
    public new UIComponent<TOwner> Parent
    {
        get => (UIComponent<TOwner>)base.Parent;
        internal set => base.Parent = value;
    }

    /// <inheritdoc/>
    public ValueProvider<bool, TOwner> IsPresent =>
        CreateValueProvider("presence state", GetIsPresent);

    /// <inheritdoc/>
    public ValueProvider<bool, TOwner> IsVisible =>
        CreateValueProvider("visible state", GetIsVisible);

    /// <inheritdoc/>
    public ValueProvider<bool, TOwner> IsVisibleInViewport =>
        CreateValueProvider("visible in viewport state", GetIsVisibleInViewport);

    /// <inheritdoc/>
    public ValueProvider<string, TOwner> TagName =>
        CreateValueProvider("tag name", () => Scope.TagName);

    /// <inheritdoc/>
    public ValueProvider<string, TOwner> Content =>
        CreateValueProvider("content", GetContent);

    /// <inheritdoc/>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public UIComponentVerificationProvider<UIComponent<TOwner>, TOwner> Should =>
        new(this);

    /// <inheritdoc/>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public UIComponentVerificationProvider<UIComponent<TOwner>, TOwner> ExpectTo =>
        Should.Using(ExpectationVerificationStrategy.Instance);

    /// <inheritdoc/>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public UIComponentVerificationProvider<UIComponent<TOwner>, TOwner> WaitTo =>
        Should.Using(WaitingVerificationStrategy.Instance);

    /// <inheritdoc/>
    public UIComponentLocationProvider<TOwner> ComponentLocation =>
        new(this, GetLocation, BuildFullValueProviderName("location"));

    /// <inheritdoc/>
    public UIComponentSizeProvider<TOwner> ComponentSize =>
        new(this, GetSize, BuildFullValueProviderName("size"));

    /// <inheritdoc/>
    public UIComponentDomAttributesProvider<TOwner> DomAttributes { get; }

    /// <inheritdoc/>
    public UIComponentDomPropertiesProvider<TOwner> DomProperties { get; }

    /// <inheritdoc/>
    public ValueProvider<IEnumerable<string>, TOwner> DomClasses =>
        CreateValueProvider<IEnumerable<string>>(
            "DOM classes",
            () => DomProperties.GetValue("className").Split([' '], StringSplitOptions.RemoveEmptyEntries));

    /// <inheritdoc/>
    public UIComponentCssProvider<TOwner> Css { get; }

    /// <inheritdoc/>
    public UIComponentScriptExecutor<TOwner> Script { get; }

    /// <inheritdoc/>
    public UIComponentChildrenList<TOwner> Controls { get; }

    IUIComponent<TOwner> IUIComponent<TOwner>.Parent => Parent;

    IScopeLocator IUIComponent<TOwner>.ScopeLocator => ScopeLocator;

    ScopeSource IUIComponent<TOwner>.ScopeSource => ScopeSource;

    internal List<IClearsCache> CacheClearableComponentParts { get; } = [];

    /// <summary>
    /// Called upon initialization before the <see cref="TriggerEvents.Init"/> triggers are executed.
    /// Calls <see cref="OnInit"/> method for all child controls.
    /// </summary>
    protected virtual void OnInit()
    {
        foreach (UIComponent<TOwner> child in Controls)
            child.OnInit();
    }

    /// <summary>
    /// Called when initialization is completed after the <see cref="TriggerEvents.Init"/> triggers are executed.
    /// Calls <see cref="OnInitCompleted"/> method for all child controls.
    /// </summary>
    protected virtual void OnInitCompleted()
    {
        foreach (UIComponent<TOwner> child in Controls)
            child.OnInitCompleted();
    }

    internal sealed override IWebElement OnGetScopeElement(SearchOptions searchOptions)
    {
        ExecuteTriggers(TriggerEvents.BeforeAccess);

        IWebElement element = ScopeLocator.GetElement(searchOptions);

        if (!searchOptions.IsSafely && element == null)
        {
            throw ElementExceptionFactory.CreateForNotFound(
                new SearchFailureData
                {
                    ElementName = ComponentFullName,
                    SearchOptions = searchOptions
                });
        }

        ExecuteTriggers(TriggerEvents.AfterAccess);

        return element;
    }

    /// <summary>
    /// Waits until the specified component condition is met.
    /// </summary>
    /// <param name="until">The waiting condition.</param>
    /// <param name="options">The options.</param>
    /// <returns>The instance of the owner page object.</returns>
    public TOwner Wait(Until until, WaitOptions options = null)
    {
        foreach (WaitUnit unit in until.GetWaitUnits(options))
        {
            Log.ExecuteSection(
                new WaitForComponentLogSection(this, unit),
                () => OnWait(unit));
        }

        return Owner;
    }

    protected virtual void OnWait(WaitUnit waitUnit) =>
        StaleSafely.Execute(
            options =>
            {
                if (waitUnit.Method == WaitUnit.WaitMethod.Presence)
                    Exists(options);
                else
                    Missing(options);
            },
            waitUnit.SearchOptions);

    internal sealed override bool OnMissing(SearchOptions options)
    {
        ExecuteTriggers(TriggerEvents.BeforeAccess);

        bool result = ScopeLocator.IsMissing(options);

        ExecuteTriggers(TriggerEvents.AfterAccess);

        return result;
    }

    /// <summary>
    /// Gets the location of the component.
    /// </summary>
    /// <returns>The size.</returns>
    protected virtual Point GetLocation() =>
        Scope.Location;

    /// <summary>
    /// Gets the size of the component.
    /// </summary>
    /// <returns>The size.</returns>
    protected virtual Size GetSize() =>
        Scope.Size;

    protected virtual bool GetIsPresent() =>
        GetScope(SearchOptions.SafelyAtOnce()) != null;

    protected virtual bool GetIsVisible() =>
        GetScope(SearchOptions.SafelyAtOnce())?.Displayed ?? false;

    protected virtual bool GetIsVisibleInViewport()
    {
        IWebElement element = GetScope(SearchOptions.SafelyAtOnce());

        return element != null && element.Displayed && Script.Execute<bool>(IsInViewportScript, element);
    }

    /// <summary>
    /// Gets the text content of the component by executing <see cref="ContentGetBehaviorAttribute"/>.
    /// </summary>
    /// <returns>The text content.</returns>
    protected virtual string GetContent() =>
        ExecuteBehavior<ContentGetBehaviorAttribute, string>(x => x.Execute(this));

    /// <summary>
    /// Gets the <see cref="ValueProvider{TValue, TOwner}"/> of the text content using <paramref name="source"/> argument.
    /// </summary>
    /// <param name="source">The source of the content.</param>
    /// <returns>The <see cref="ValueProvider{TValue, TOwner}"/> of the text content.</returns>
    public ValueProvider<string, TOwner> GetContent(ContentSource source) =>
        CreateValueProvider(
            $"content ({source.ToString(TermCase.MidSentence)})",
            () => ContentExtractor.Get(this, source));

    ValueProvider<TValue, TOwner> IUIComponent<TOwner>.CreateValueProvider<TValue>(string providerName, Func<TValue> valueGetFunction) =>
        CreateValueProvider(providerName, valueGetFunction);

    /// <inheritdoc cref="IUIComponent{TOwner}.CreateValueProvider{TValue}(string, Func{TValue})"/>
    protected internal ValueProvider<TValue, TOwner> CreateValueProvider<TValue>(string providerName, Func<TValue> valueGetFunction)
    {
        string fullProviderName = BuildFullValueProviderName(providerName);

        return new ValueProvider<TValue, TOwner>(
            Owner,
            DynamicObjectSource.Create(valueGetFunction),
            fullProviderName,
            Session.ExecutionUnit);
    }

    EnumerableValueProvider<TItem, TOwner> IUIComponent<TOwner>.CreateEnumerableValueProvider<TItem>(string providerName, Func<IEnumerable<TItem>> valueGetFunction) =>
        CreateEnumerableValueProvider(providerName, valueGetFunction);

    /// <inheritdoc cref="IUIComponent{TOwner}.CreateEnumerableValueProvider{TItem}(string, Func{IEnumerable{TItem}})"/>
    protected internal EnumerableValueProvider<TItem, TOwner> CreateEnumerableValueProvider<TItem>(string providerName, Func<IEnumerable<TItem>> valueGetFunction)
    {
        string fullProviderName = BuildFullValueProviderName(providerName);

        return new EnumerableValueProvider<TItem, TOwner>(
            Owner,
            DynamicObjectSource.Create(valueGetFunction),
            fullProviderName,
            Session.ExecutionUnit);
    }

    protected string BuildFullValueProviderName(string providerName)
    {
        string componentProviderName = BuildComponentProviderName();

        return string.IsNullOrEmpty(componentProviderName)
            ? providerName
            : $"{componentProviderName} {providerName}";
    }

    protected virtual string BuildComponentProviderName() =>
        ComponentFullName;

    /// <summary>
    /// Executes the triggers.
    /// </summary>
    /// <param name="on">The event to trigger.</param>
    protected void ExecuteTriggers(TriggerEvents on)
    {
        if (on == TriggerEvents.None || _currentDeniedTriggers.Contains(on))
            return;

        var orderedTriggers = Metadata.GetAll<TriggerAttribute>().OrderBy(x => x.Priority).ToArray();

        if (orderedTriggers.Length > 0)
        {
            if (DenyTriggersMap.Values.TryGetValue(on, out TriggerEvents[] denyTriggers))
                _currentDeniedTriggers.AddRange(denyTriggers);

            try
            {
                var triggers = orderedTriggers.Where(x => x.On.HasFlag(on));

                TriggerContext<TOwner> context = new TriggerContext<TOwner>
                {
                    Event = on,
                    Driver = Driver,
                    Log = Log,
                    Component = this
                };

                foreach (var trigger in triggers)
                {
                    Log.ExecuteSection(
                        new ExecuteTriggerLogSection(this, trigger, on),
                        () => trigger.Execute(context));
                }
            }
            finally
            {
                if (denyTriggers != null)
                    _currentDeniedTriggers.RemoveAll(x => denyTriggers.Contains(x));
            }
        }

        if (on is TriggerEvents.Init or TriggerEvents.DeInit)
            foreach (UIComponent<TOwner> child in Controls)
                child.ExecuteTriggers(on);
    }

    /// <inheritdoc/>
    public TControl Find<TControl>(params Attribute[] attributes)
        where TControl : Control<TOwner>
        =>
        Find<TControl>(null, attributes);

    /// <inheritdoc/>
    public TControl Find<TControl>(string name, params Attribute[] attributes)
        where TControl : Control<TOwner>
        =>
        UIComponentResolver.CreateControl<TControl, TOwner>(this, name, attributes);

    public ControlList<TControl, TOwner> FindAll<TControl>(params Attribute[] attributes)
        where TControl : Control<TOwner>
        =>
        FindAll<TControl>(
            $"{UIComponentResolver.ResolveControlTypeName<TControl>()} items",
            attributes);

    public ControlList<TControl, TOwner> FindAll<TControl>(string name, params Attribute[] attributes)
        where TControl : Control<TOwner>
        =>
        UIComponentResolver.CreateComponentPart<ControlList<TControl, TOwner>, TOwner>(this, name, attributes);

    /// <summary>
    /// Gets the ancestor component of specified type.
    /// </summary>
    /// <typeparam name="TComponentToFind">The type of the component to find.</typeparam>
    /// <returns>The component or <see langword="null"/> if not found.</returns>
    public TComponentToFind GetAncestor<TComponentToFind>()
        where TComponentToFind : UIComponent<TOwner>
        =>
        (Parent as TComponentToFind) ?? Parent?.GetAncestor<TComponentToFind>();

    /// <summary>
    /// Gets the ancestor component of specified type or self.
    /// </summary>
    /// <typeparam name="TComponentToFind">The type of the component to find.</typeparam>
    /// <returns>The component or <see langword="null"/> if not found.</returns>
    public TComponentToFind GetAncestorOrSelf<TComponentToFind>()
        where TComponentToFind : UIComponent<TOwner>
        =>
        (this as TComponentToFind) ?? Parent?.GetAncestorOrSelf<TComponentToFind>();

    /// <inheritdoc/>
    public TOwner ClearCache()
    {
        OnClearCache();

        return ClearCacheOfDescendants();
    }

    /// <inheritdoc/>
    public TOwner ClearCacheOfDescendants()
    {
        foreach (var componentPart in CacheClearableComponentParts)
            componentPart.ClearCache();

        foreach (var control in Controls)
            control.ClearCache();

        return Owner;
    }

    /// <summary>
    /// Clears the cache of the component.
    /// </summary>
    protected virtual void OnClearCache() =>
        ClearScopeCache();

    /// <inheritdoc/>
    public TOwner ClearScopeCache()
    {
        var cachedScope = CachedScope;

        if (cachedScope != null)
        {
            CachedScope = null;
            Log.Trace($"Cleared scope cache of {ComponentFullName}: {Stringifier.ToString(cachedScope)}");
        }

        return Owner;
    }

    /// <summary>
    /// Cleans up the current instance.
    /// </summary>
    protected internal override void CleanUp()
    {
        ClearCache();

        foreach (var control in Controls)
            control.CleanUp();

        Controls.Clear();
    }
}
