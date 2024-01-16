namespace Atata;

/// <summary>
/// Represents an interface for UI component.
/// </summary>
/// <typeparam name="TOwner">The type of the owner page object.</typeparam>
public interface IUIComponent<TOwner>
    where TOwner : PageObject<TOwner>
{
    /// <summary>
    /// Gets the <see cref="AtataContext"/> instance with which this component is associated.
    /// </summary>
    AtataContext Context { get; }

    /// <summary>
    /// Gets the owner page object.
    /// </summary>
    TOwner Owner { get; }

    /// <summary>
    /// Gets the parent component.
    /// </summary>
    IUIComponent<TOwner> Parent { get; }

    /// <summary>
    /// Gets the <see cref="ValueProvider{TValue, TOwner}"/> of a value indicating
    /// whether the component is present considering the <see cref="Visibility"/> of component.
    /// </summary>
    ValueProvider<bool, TOwner> IsPresent { get; }

    /// <summary>
    /// Gets the <see cref="ValueProvider{TValue, TOwner}"/> of a value indicating
    /// whether the component is visible.
    /// </summary>
    ValueProvider<bool, TOwner> IsVisible { get; }

    /// <summary>
    /// Gets the <see cref="ValueProvider{TValue, TOwner}"/> of a value indicating
    /// whether the component is visible in viewport.
    /// </summary>
    ValueProvider<bool, TOwner> IsVisibleInViewport { get; }

    /// <summary>
    /// Gets the <see cref="ValueProvider{TValue, TOwner}"/> of the scope element tag name.
    /// </summary>
    ValueProvider<string, TOwner> TagName { get; }

    /// <summary>
    /// Gets the <see cref="ValueProvider{TValue, TOwner}"/> of the text content.
    /// Gets a content using <see cref="ContentGetBehaviorAttribute"/> associated with the component,
    /// which by default is <see cref="GetsContentFromSourceAttribute"/> with <see cref="ContentSource.Text"/> argument,
    /// meaning that by default it returns <see cref="IWebElement.Text"/> property value
    /// of component scope's <see cref="IWebElement"/> element.
    /// </summary>
    ValueProvider<string, TOwner> Content { get; }

    /// <summary>
    /// Gets the source of the scope, e.g., <see cref="ScopeSource.Parent"/>, <see cref="ScopeSource.Page"/>, etc.
    /// </summary>
    ScopeSource ScopeSource { get; }

    /// <summary>
    /// Gets the <see cref="IWebElement"/> instance that represents the scope HTML element associated with this component.
    /// Also executes <see cref="TriggerEvents.BeforeAccess" /> and <see cref="TriggerEvents.AfterAccess" /> triggers.
    /// </summary>
    /// <exception cref="ElementNotFoundException">Element not found.</exception>
    IWebElement Scope { get; }

    /// <summary>
    /// Gets the <see cref="ISearchContext"/> instance that represents the scope search context
    /// (where to find the children of this component).
    /// By default is the same as <see cref="Scope"/>.
    /// Also can execute <see cref="TriggerEvents.BeforeAccess" /> and <see cref="TriggerEvents.AfterAccess" /> triggers.
    /// </summary>
    ISearchContext ScopeContext { get; }

    /// <summary>
    /// Gets the scope locator.
    /// </summary>
    IScopeLocator ScopeLocator { get; }

    /// <summary>
    /// Gets or sets the name of the component.
    /// </summary>
    string ComponentName { get; set; }

    /// <summary>
    /// Gets the name of the component type.
    /// Returns the value of <see cref="UIComponentDefinitionAttribute.ComponentTypeName"/> property for control from <see cref="ControlDefinitionAttribute"/>
    /// and for page object from <see cref="PageObjectDefinitionAttribute"/>.
    /// </summary>
    string ComponentTypeName { get; }

    /// <summary>
    /// Gets the full name of the component including parent component full name, own component name and own component type name.
    /// </summary>
    string ComponentFullName { get; }

    /// <summary>
    /// Gets the <see cref="UIComponentLocationProvider{TOwner}"/> instance that provides an access to the scope element's location (X and Y).
    /// </summary>
    UIComponentLocationProvider<TOwner> ComponentLocation { get; }

    /// <summary>
    /// Gets the <see cref="UIComponentSizeProvider{TOwner}"/> instance that provides an access to the scope element's size (Width and Height).
    /// </summary>
    UIComponentSizeProvider<TOwner> ComponentSize { get; }

    /// <summary>
    /// Gets the <see cref="UIComponentDomAttributesProvider{TOwner}"/> instance
    /// that provides an access to the scope element's DOM attributes.
    /// </summary>
    UIComponentDomAttributesProvider<TOwner> DomAttributes { get; }

    /// <summary>
    /// Gets the <see cref="UIComponentDomPropertiesProvider{TOwner}"/> instance
    /// that provides an access to the scope element's DOM properties.
    /// </summary>
    UIComponentDomPropertiesProvider<TOwner> DomProperties { get; }

    /// <summary>
    /// Gets the <see cref="ValueProvider{TValue, TOwner}"/> instance
    /// that provides a list of the scope element's DOM classes.
    /// </summary>
    ValueProvider<IEnumerable<string>, TOwner> DomClasses { get; }

    /// <summary>
    /// Gets the <see cref="UIComponentCssProvider{TOwner}"/> instance
    /// that provides an access to the scope element's CSS properties.
    /// </summary>
    UIComponentCssProvider<TOwner> Css { get; }

    /// <summary>
    /// Gets the <see cref="UIComponentScriptExecutor{TOwner}"/> instance that provides a set of methods for JavaScript execution.
    /// </summary>
    UIComponentScriptExecutor<TOwner> Script { get; }

    /// <summary>
    /// Gets the list of child controls.
    /// </summary>
    UIComponentChildrenList<TOwner> Controls { get; }

    /// <summary>
    /// Gets the metadata of the component.
    /// It is possible to query/add/remove attributes in metadata at any moment.
    /// </summary>
    UIComponentMetadata Metadata { get; }

    /// <summary>
    /// Gets the assertion verification provider that has a set of verification extension methods.
    /// </summary>
    UIComponentVerificationProvider<UIComponent<TOwner>, TOwner> Should { get; }

    /// <summary>
    /// Gets the expectation verification provider that has a set of verification extension methods.
    /// </summary>
    UIComponentVerificationProvider<UIComponent<TOwner>, TOwner> ExpectTo { get; }

    /// <summary>
    /// Gets the waiting verification provider that has a set of verification extension methods.
    /// Uses <see cref="AtataContext.WaitingTimeout"/> and <see cref="AtataContext.WaitingRetryInterval"/> of <see cref="AtataContext.Current"/> for timeout and retry interval.
    /// </summary>
    UIComponentVerificationProvider<UIComponent<TOwner>, TOwner> WaitTo { get; }

    /// <summary>
    /// Gets the <see cref="IWebElement"/> instance that represents the scope HTML element.
    /// Also executes <see cref="TriggerEvents.BeforeAccess" /> and <see cref="TriggerEvents.AfterAccess" /> triggers.
    /// </summary>
    /// <param name="options">The search options.
    /// If set to <see langword="null"/>, then it uses <c>SearchOptions.Safely()</c>.</param>
    /// <returns>The <see cref="IWebElement"/> instance of the scope.</returns>
    IWebElement GetScope(SearchOptions options = null);

    /// <summary>
    /// Gets the <see cref="ISearchContext"/> instance that represents the scope search context
    /// (where to find the children of this component).
    /// Also can execute <see cref="TriggerEvents.BeforeAccess" /> and <see cref="TriggerEvents.AfterAccess" /> triggers.
    /// </summary>
    /// <param name="options">
    /// The search options.
    /// If set to <see langword="null"/>, then it uses <c>SearchOptions.Safely()</c>.</param>
    /// <returns>The <see cref="ISearchContext"/> instance of the scope context.</returns>
    ISearchContext GetScopeContext(SearchOptions options = null);

    /// <summary>
    /// Waits until the specified component condition is met.
    /// </summary>
    /// <param name="until">The waiting condition.</param>
    /// <param name="options">The options.</param>
    /// <returns>The instance of the owner page object.</returns>
    TOwner Wait(Until until, WaitOptions options = null);

    /// <summary>
    /// Determines whether the component exists.
    /// </summary>
    /// <param name="options">The search options.
    /// If set to <see langword="null"/>, then it uses <c>SearchOptions.Safely()</c>.</param>
    /// <returns><see langword="true"/> if the component exists; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="ElementNotFoundException">
    /// The <paramref name="options"/> has <see cref="SearchOptions.IsSafely"/> property
    /// equal to <see langword="false"/> value and the component doesn't exist.
    /// </exception>
    bool Exists(SearchOptions options = null);

    /// <summary>
    /// Determines whether the component is missing.
    /// </summary>
    /// <param name="options">The search options.
    /// If set to <see langword="null"/>, then it uses <c>SearchOptions.Safely()</c>.</param>
    /// <returns><see langword="true"/> if the component is missing; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="ElementNotMissingException">
    /// The <paramref name="options"/> has <see cref="SearchOptions.IsSafely"/> property
    /// equal to <see langword="false"/> value and the component exists.
    /// </exception>
    bool Missing(SearchOptions options = null);

    /// <summary>
    /// Creates a value provider with the specified <paramref name="providerName"/> and using <paramref name="valueGetFunction"/>.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="providerName">The name of the provider.</param>
    /// <param name="valueGetFunction">The function that gets a value.</param>
    /// <returns>A new instance of <see cref="ValueProvider{TValue, TOwner}"/> type.</returns>
    ValueProvider<TValue, TOwner> CreateValueProvider<TValue>(string providerName, Func<TValue> valueGetFunction);

    /// <summary>
    /// Creates an enumerable value provider with the specified <paramref name="providerName"/> and using <paramref name="valueGetFunction"/>.
    /// </summary>
    /// <typeparam name="TItem">The type of the enumerable item.</typeparam>
    /// <param name="providerName">The name of the provider.</param>
    /// <param name="valueGetFunction">The function that gets a value.</param>
    /// <returns>A new instance of <see cref="EnumerableValueProvider{TItem, TOwner}"/> type.</returns>
    EnumerableValueProvider<TItem, TOwner> CreateEnumerableValueProvider<TItem>(string providerName, Func<IEnumerable<TItem>> valueGetFunction);

    /// <summary>
    /// Creates a control of the specified <typeparamref name="TControl"/> type,
    /// optionally with additional attributes, that is a descendant of the current component.
    /// The control's element will be found using either
    /// the <see cref="FindAttribute"/> specified in <paramref name="attributes"/> parameter,
    /// or the default/applied <see cref="FindAttribute"/> associated with the <typeparamref name="TControl"/> type.
    /// </summary>
    /// <typeparam name="TControl">The type of the control.</typeparam>
    /// <param name="attributes">The attributes.</param>
    /// <returns>The created control instance.</returns>
    TControl Find<TControl>(params Attribute[] attributes)
        where TControl : Control<TOwner>;

    /// <summary>
    /// Creates a control of the specified <typeparamref name="TControl"/> type with the specified name,
    /// optionally with additional attributes, that is a descendant of the current component.
    /// The control's element will be found using either
    /// the <see cref="FindAttribute"/> specified in <paramref name="attributes"/> parameter,
    /// or the default/applied <see cref="FindAttribute"/> associated with the <typeparamref name="TControl"/> type.
    /// </summary>
    /// <typeparam name="TControl">The type of the control.</typeparam>
    /// <param name="name">The control name, which is used in log.</param>
    /// <param name="attributes">The attributes.</param>
    /// <returns>The created control instance.</returns>
    TControl Find<TControl>(string name, params Attribute[] attributes)
        where TControl : Control<TOwner>;

    /// <summary>
    /// Creates a control list of the specified <typeparamref name="TControl"/> type,
    /// optionally with additional attributes, that are descendants of the current component.
    /// Use <see cref="ControlDefinitionAttribute"/> to specialize the control element definition,
    /// instead of <see cref="FindAttribute"/> that doesn't utilize here.
    /// </summary>
    /// <typeparam name="TControl">The type of the control.</typeparam>
    /// <param name="attributes">The attributes.</param>
    /// <returns>The instance of <see cref="ControlList{TItem, TOwner}"/>.</returns>
    ControlList<TControl, TOwner> FindAll<TControl>(params Attribute[] attributes)
        where TControl : Control<TOwner>;

    /// <summary>
    /// Creates a control list of the specified <typeparamref name="TControl"/> type with the specified name,
    /// optionally with additional attributes, that are descendants of the current component.
    /// Use <see cref="ControlDefinitionAttribute"/> to specialize the control element definition,
    /// instead of <see cref="FindAttribute"/> that doesn't utilize here.
    /// </summary>
    /// <typeparam name="TControl">The type of the control.</typeparam>
    /// <param name="name">The control list name, which is used in log.</param>
    /// <param name="attributes">The attributes.</param>
    /// <returns>The instance of <see cref="ControlList{TItem, TOwner}"/>.</returns>
    ControlList<TControl, TOwner> FindAll<TControl>(string name, params Attribute[] attributes)
        where TControl : Control<TOwner>;

    /// <summary>
    /// Gets the ancestor component of specified type.
    /// </summary>
    /// <typeparam name="TComponentToFind">The type of the component to find.</typeparam>
    /// <returns>The component or <see langword="null"/> if not found.</returns>
    TComponentToFind GetAncestor<TComponentToFind>()
        where TComponentToFind : UIComponent<TOwner>;

    /// <summary>
    /// Gets the ancestor component of specified type or self.
    /// </summary>
    /// <typeparam name="TComponentToFind">The type of the component to find.</typeparam>
    /// <returns>The component or <see langword="null"/> if not found.</returns>
    TComponentToFind GetAncestorOrSelf<TComponentToFind>()
        where TComponentToFind : UIComponent<TOwner>;

    /// <summary>
    /// Gets a behavior attribute from the component's metadata and then invokes
    /// the specified <paramref name="behaviorExecutionAction" /> with passing the found behavior to it.
    /// </summary>
    /// <typeparam name="TBehaviorAttribute">The type of the behavior attribute.</typeparam>
    /// <param name="behaviorExecutionAction">The behavior execution action.</param>
    void ExecuteBehavior<TBehaviorAttribute>(Action<TBehaviorAttribute> behaviorExecutionAction)
        where TBehaviorAttribute : MulticastAttribute;

    /// <summary>
    /// Gets a behavior attribute from the component's metadata and then invokes
    /// the specified <paramref name="behaviorExecutionFunction" /> with passing the found behavior to it.
    /// </summary>
    /// <typeparam name="TBehaviorAttribute">The type of the behavior attribute.</typeparam>
    /// <typeparam name="TResult">The type of the behavior result.</typeparam>
    /// <param name="behaviorExecutionFunction">The behavior execution function.</param>
    /// <returns>The result of behavior execution.</returns>
    TResult ExecuteBehavior<TBehaviorAttribute, TResult>(Func<TBehaviorAttribute, TResult> behaviorExecutionFunction)
        where TBehaviorAttribute : MulticastAttribute;

    /// <summary>
    /// Clears all cache of the component and its descendant components.
    /// </summary>
    /// <returns>The instance of the owner page object.</returns>
    TOwner ClearCache();

    /// <summary>
    /// Clears all cache of the descendant components.
    /// </summary>
    /// <returns>The instance of the owner page object.</returns>
    TOwner ClearCacheOfDescendants();

    /// <summary>
    /// Clears the scope cache of the component.
    /// </summary>
    /// <returns>The instance of the owner page object.</returns>
    TOwner ClearScopeCache();
}
