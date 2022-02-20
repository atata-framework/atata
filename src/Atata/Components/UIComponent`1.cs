using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OpenQA.Selenium;

namespace Atata
{
    /// <summary>
    /// Represents the base class for UI components.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [GetsContentFromSource(ContentSource.Text)]
    public abstract class UIComponent<TOwner> : UIComponent, IUIComponent<TOwner>
        where TOwner : PageObject<TOwner>
    {
        private const string IsInViewPortScript = @"
const element = arguments[0];                
const rect = element.getBoundingClientRect();

return (
  rect.top >= 0 &&
  rect.left >= 0 &&
  Math.floor(rect.bottom) <= (window.innerHeight || document.documentElement.clientHeight) &&
  Math.floor(rect.right) <= (window.innerWidth || document.documentElement.clientWidth));";

        private readonly Dictionary<string, object> _dataProviders = new Dictionary<string, object>();

        private readonly List<TriggerEvents> _currentDeniedTriggers = new List<TriggerEvents>();

        protected UIComponent()
        {
            Controls = new UIComponentChildrenList<TOwner>(this);
            Attributes = new UIComponentAttributeProvider<TOwner> { Component = this, ComponentPartName = "attributes" };
            Css = new UIComponentCssProvider<TOwner> { Component = this, ComponentPartName = "CSS" };
            Script = new UIComponentScriptExecutor<TOwner>(this);
            ComponentLocation = new UIComponentLocationProvider<TOwner>(this, GetLocation);
            ComponentSize = new UIComponentSizeProvider<TOwner>(this, GetSize);
        }

        /// <summary>
        /// Gets the owner page object.
        /// </summary>
        public new TOwner Owner
        {
            get { return (TOwner)base.Owner; }
            internal set { base.Owner = value; }
        }

        /// <summary>
        /// Gets the parent component.
        /// </summary>
        public new UIComponent<TOwner> Parent
        {
            get { return (UIComponent<TOwner>)base.Parent; }
            internal set { base.Parent = value; }
        }

        /// <summary>
        /// Gets the <see cref="ValueProvider{TValue, TOwner}"/> of a value indicating
        /// whether the component is present considering the <see cref="Visibility"/> of component.
        /// </summary>
        public ValueProvider<bool, TOwner> IsPresent =>
            CreateValueProvider("presence state", GetIsPresent);

        /// <summary>
        /// Gets the <see cref="ValueProvider{TValue, TOwner}"/> of a value indicating
        /// whether the component is visible.
        /// </summary>
        public ValueProvider<bool, TOwner> IsVisible =>
            CreateValueProvider("visible state", GetIsVisible);

        /// <summary>
        /// Gets the <see cref="ValueProvider{TValue, TOwner}"/> of a value indicating
        /// whether the component is visible in viewport.
        /// </summary>
        public ValueProvider<bool, TOwner> IsVisibleInViewPort =>
            CreateValueProvider("visible in viewport state", GetIsVisibleInViewPort);

        /// <summary>
        /// Gets the <see cref="ValueProvider{TValue, TOwner}"/> of the text content.
        /// Gets a content using <see cref="ContentGetBehaviorAttribute"/> associated with the component,
        /// which by default is <see cref="GetsContentFromSourceAttribute"/> with <see cref="ContentSource.Text"/> argument,
        /// meaning that by default it returns <see cref="IWebElement.Text"/> property value
        /// of component scope's <see cref="IWebElement"/> element.
        /// </summary>
        public ValueProvider<string, TOwner> Content =>
            CreateValueProvider("content", GetContent);

        /// <summary>
        /// Gets the assertion verification provider that has a set of verification extension methods.
        /// </summary>
        public UIComponentVerificationProvider<UIComponent<TOwner>, TOwner> Should =>
            new UIComponentVerificationProvider<UIComponent<TOwner>, TOwner>(this);

        /// <summary>
        /// Gets the expectation verification provider that has a set of verification extension methods.
        /// </summary>
        public UIComponentVerificationProvider<UIComponent<TOwner>, TOwner> ExpectTo =>
            Should.Using<ExpectationVerificationStrategy>();

        /// <summary>
        /// Gets the waiting verification provider that has a set of verification extension methods.
        /// Uses <see cref="AtataContext.WaitingTimeout"/> and <see cref="AtataContext.WaitingRetryInterval"/> of <see cref="AtataContext.Current"/> for timeout and retry interval.
        /// </summary>
        public UIComponentVerificationProvider<UIComponent<TOwner>, TOwner> WaitTo =>
            Should.Using<WaitingVerificationStrategy>();

        /// <summary>
        /// Gets the <see cref="UIComponentLocationProvider{TOwner}"/> instance that provides an access to the scope element's location (X and Y).
        /// </summary>
        public UIComponentLocationProvider<TOwner> ComponentLocation { get; }

        /// <summary>
        /// Gets the <see cref="UIComponentSizeProvider{TOwner}"/> instance that provides an access to the scope element's size (Width and Height).
        /// </summary>
        public UIComponentSizeProvider<TOwner> ComponentSize { get; }

        /// <summary>
        /// Gets the <see cref="UIComponentAttributeProvider{TOwner}"/> instance that provides an access to the scope element's attributes.
        /// </summary>
        public UIComponentAttributeProvider<TOwner> Attributes { get; }

        /// <summary>
        /// Gets the <see cref="UIComponentCssProvider{TOwner}"/> instance that provides an access to the scope element's CSS properties.
        /// </summary>
        public UIComponentCssProvider<TOwner> Css { get; }

        /// <summary>
        /// Gets the <see cref="UIComponentScriptExecutor{TOwner}"/> instance that provides a set of methods for JavaScript execution.
        /// </summary>
        public UIComponentScriptExecutor<TOwner> Script { get; }

        IUIComponent<TOwner> IUIComponent<TOwner>.Parent => Parent;

        IScopeLocator IUIComponent<TOwner>.ScopeLocator => ScopeLocator;

        ScopeSource IUIComponent<TOwner>.ScopeSource => ScopeSource;

        /// <summary>
        /// Gets the list of child controls.
        /// </summary>
        public UIComponentChildrenList<TOwner> Controls { get; }

        internal List<IClearsCache> CacheClearableComponentParts { get; } = new List<IClearsCache>();

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

        protected internal virtual void InitComponent()
        {
            UIComponentResolver.Resolve(this);
        }

        internal sealed override IWebElement OnGetScopeElement(SearchOptions searchOptions)
        {
            ExecuteTriggers(TriggerEvents.BeforeAccess);

            IWebElement element = ScopeLocator.GetElement(searchOptions);

            if (!searchOptions.IsSafely && element == null)
            {
                throw ExceptionFactory.CreateForNoSuchElement(
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

        protected virtual void OnWait(WaitUnit waitUnit)
        {
            StaleSafely.Execute(
                options =>
                {
                    if (waitUnit.Method == WaitUnit.WaitMethod.Presence)
                        Exists(options);
                    else
                        Missing(options);
                },
                waitUnit.SearchOptions);
        }

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

        protected virtual bool GetIsPresent()
        {
            return GetScope(SearchOptions.SafelyAtOnce()) != null;
        }

        protected virtual bool GetIsVisible()
        {
            return GetScope(SearchOptions.SafelyAtOnce())?.Displayed ?? false;
        }

        protected virtual bool GetIsVisibleInViewPort()
        {
            IWebElement element = GetScope(SearchOptions.SafelyAtOnce());

            return element != null && element.Displayed && Script.Execute<bool>(IsInViewPortScript, element);
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

        /// <summary>
        /// Gets the data provider by name or creates and stores a new instance with the specified <paramref name="providerName"/> and using <paramref name="valueGetFunction"/>.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="providerName">Name of the provider.</param>
        /// <param name="valueGetFunction">The function that gets a value.</param>
        /// <returns>A new instance of <see cref="DataProvider{TData, TOwner}"/> type or already stored one.</returns>
        public DataProvider<TValue, TOwner> GetOrCreateDataProvider<TValue>(string providerName, Func<TValue> valueGetFunction)
        {
            if (_dataProviders.TryGetValue(providerName, out object dataProviderAsObject) && dataProviderAsObject is DataProvider<TValue, TOwner> dataProvider)
                return dataProvider;

            return CreateDataProvider(providerName, valueGetFunction);
        }

        /// <summary>
        /// Creates and stores a new instance with the specified <paramref name="providerName"/> and using <paramref name="valueGetFunction"/>.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="providerName">Name of the provider.</param>
        /// <param name="valueGetFunction">The function that gets a value.</param>
        /// <returns>A new instance of <see cref="DataProvider{TData, TOwner}"/> type.</returns>
        protected internal DataProvider<TValue, TOwner> CreateDataProvider<TValue>(string providerName, Func<TValue> valueGetFunction)
        {
            string componentProviderName = BuildComponentProviderName();
            string fullProviderName = string.IsNullOrEmpty(componentProviderName)
                ? providerName
                : $"{componentProviderName} {providerName}";

            var dataProvider = new DataProvider<TValue, TOwner>(this, valueGetFunction, fullProviderName);
            _dataProviders[providerName] = dataProvider;
            return dataProvider;
        }

        ValueProvider<TValue, TOwner> IUIComponent<TOwner>.CreateValueProvider<TValue>(string providerName, Func<TValue> valueGetFunction) =>
            CreateValueProvider(providerName, valueGetFunction);

        /// <inheritdoc cref="IUIComponent{TOwner}.CreateValueProvider{TValue}(string, Func{TValue})"/>
        protected internal ValueProvider<TValue, TOwner> CreateValueProvider<TValue>(string providerName, Func<TValue> valueGetFunction)
        {
            string componentProviderName = BuildComponentProviderName();
            string fullProviderName = string.IsNullOrEmpty(componentProviderName)
                ? providerName
                : $"{componentProviderName} {providerName}";

            return new ValueProvider<TValue, TOwner>(
                Owner,
                DynamicObjectSource.Create(valueGetFunction),
                fullProviderName);
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

            if (orderedTriggers.Any())
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

            if (on == TriggerEvents.Init || on == TriggerEvents.DeInit)
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
        {
            return (Parent as TComponentToFind) ?? Parent?.GetAncestor<TComponentToFind>();
        }

        /// <summary>
        /// Gets the ancestor component of specified type or self.
        /// </summary>
        /// <typeparam name="TComponentToFind">The type of the component to find.</typeparam>
        /// <returns>The component or <see langword="null"/> if not found.</returns>
        public TComponentToFind GetAncestorOrSelf<TComponentToFind>()
            where TComponentToFind : UIComponent<TOwner>
        {
            return (this as TComponentToFind) ?? Parent?.GetAncestorOrSelf<TComponentToFind>();
        }

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
}
