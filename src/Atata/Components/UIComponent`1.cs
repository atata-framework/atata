using System;
using System.Collections.Generic;
using OpenQA.Selenium;

namespace Atata
{
    /// <summary>
    /// Represents the base class for UI components.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ContentSource(ContentSource.Text)]
    public abstract class UIComponent<TOwner> : UIComponent, IUIComponent<TOwner>
        where TOwner : PageObject<TOwner>
    {
        private const string IsInViewPortScript = @"
const element = arguments[0];                
const rect = element.getBoundingClientRect();

return (
  rect.top >= 0 &&
  rect.left >= 0 &&
  rect.bottom <= (window.innerHeight || document. documentElement.clientHeight) &&
  rect.right <= (window.innerWidth || document. documentElement.clientWidth)
);";

        private readonly Dictionary<string, object> dataProviders = new Dictionary<string, object>();

        protected UIComponent()
        {
            Controls = new UIComponentChildrenList<TOwner>(this);
            Attributes = new UIComponentAttributeProvider<TOwner> { Component = this, ComponentPartName = "attributes" };
            Css = new UIComponentCssProvider<TOwner> { Component = this, ComponentPartName = "CSS" };
            ComponentLocation = new UIComponentLocationProvider<TOwner> { Component = this, ComponentPartName = "location" };
            ComponentSize = new UIComponentSizeProvider<TOwner> { Component = this, ComponentPartName = "size" };
            Triggers = new UIComponentTriggerSet<TOwner>(this);
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
        /// Gets the <see cref="DataProvider{TData, TOwner}"/> instance for the value indicating whether the component is present considering the <see cref="Visibility"/> of component.
        /// </summary>
        public DataProvider<bool, TOwner> IsPresent => GetOrCreateDataProvider("presence state", GetIsPresent);

        /// <summary>
        /// Gets the <see cref="DataProvider{TData, TOwner}"/> instance for the value indicating whether the component is visible.
        /// </summary>
        public DataProvider<bool, TOwner> IsVisible => GetOrCreateDataProvider("visible state", GetIsVisible);

        /// <summary>
        /// Gets the <see cref="DataProvider{TData, TOwner}"/> instance for the value indicating whether the component is visible in viewport.
        /// </summary>
        public DataProvider<bool, TOwner> IsVisibleInViewPort => GetOrCreateDataProvider("visible in viewport state", GetIsVisibleInViewPort);

        /// <summary>
        /// Gets the <see cref="DataProvider{TData, TOwner}"/> instance for the text content.
        /// Gets a content using <see cref="ContentGetBehaviorAttribute"/> associated with the component,
        /// which by default is <see cref="Atata.ContentSourceAttribute"/> with <see cref="ContentSource.Text"/> argument.
        /// Meaning that be default it returns <see cref="IWebElement.Text"/> property value of component scope's <see cref="IWebElement"/> element.
        /// </summary>
        public DataProvider<string, TOwner> Content => GetOrCreateDataProvider("content", GetContent);

        /// <summary>
        /// Gets the assertion verification provider that has a set of verification extension methods.
        /// </summary>
        public UIComponentVerificationProvider<UIComponent<TOwner>, TOwner> Should => new UIComponentVerificationProvider<UIComponent<TOwner>, TOwner>(this);

        /// <summary>
        /// Gets the expectation verification provider that has a set of verification extension methods.
        /// </summary>
        public UIComponentVerificationProvider<UIComponent<TOwner>, TOwner> ExpectTo => Should.Using<ExpectationVerificationStrategy>();

        /// <summary>
        /// Gets the waiting verification provider that has a set of verification extension methods.
        /// Uses <see cref="AtataContext.WaitingTimeout"/> and <see cref="AtataContext.WaitingRetryInterval"/> of <see cref="AtataContext.Current"/> for timeout and retry interval.
        /// </summary>
        public UIComponentVerificationProvider<UIComponent<TOwner>, TOwner> WaitTo => Should.Using<WaitingVerificationStrategy>();

        /// <summary>
        /// Gets the <see cref="UIComponentLocationProvider{TOwner}"/> instance that provides an access to the scope element's location (X and Y).
        /// </summary>
        public UIComponentLocationProvider<TOwner> ComponentLocation { get; private set; }

        /// <summary>
        /// Gets the <see cref="UIComponentSizeProvider{TOwner}"/> instance that provides an access to the scope element's size (Width and Height).
        /// </summary>
        public UIComponentSizeProvider<TOwner> ComponentSize { get; private set; }

        /// <summary>
        /// Gets the <see cref="UIComponentAttributeProvider{TOwner}"/> instance that provides an access to the scope element's attributes.
        /// </summary>
        public UIComponentAttributeProvider<TOwner> Attributes { get; private set; }

        /// <summary>
        /// Gets the <see cref="UIComponentCssProvider{TOwner}"/> instance that provides an access to the scope element's CSS properties.
        /// </summary>
        public UIComponentCssProvider<TOwner> Css { get; private set; }

        IUIComponent<TOwner> IUIComponent<TOwner>.Parent => Parent;

        IScopeLocator IUIComponent<TOwner>.ScopeLocator => ScopeLocator;

        ScopeSource IUIComponent<TOwner>.ScopeSource => ScopeSource;

        /// <summary>
        /// Gets the list of child controls.
        /// </summary>
        public UIComponentChildrenList<TOwner> Controls { get; private set; }

        /// <summary>
        /// Gets the set of triggers.
        /// Provides the functionality to get/add/remove triggers dynamically.
        /// </summary>
        public UIComponentTriggerSet<TOwner> Triggers { get; internal set; }

        /// <summary>
        /// Gets an instance of <see cref="ContentSourceAttribute"/> or <see langword="null"/> if not found.
        /// </summary>
        [Obsolete("Use ContentGetBehavior instead.")] // Obsolete since v1.1.0.
        protected ContentSourceAttribute ContentSourceAttribute => Metadata.Get<ContentSourceAttribute>();

        /// <summary>
        /// Gets an instance of <see cref="ContentGetBehaviorAttribute"/> associated with the component.
        /// </summary>
        public ContentGetBehaviorAttribute ContentGetBehavior => Metadata.Get<ContentGetBehaviorAttribute>();

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
                Log.Start(new WaitForComponentLogSection(this, unit));

                OnWait(unit);

                Log.EndSection();
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

            return element != null && element.Displayed && (bool)Owner.Driver.ExecuteScript(IsInViewPortScript, element);
        }

        protected virtual string GetContent()
        {
            return ContentGetBehavior.Execute(this);
        }

        /// <summary>
        /// Gets the <see cref="DataProvider{TData, TOwner}"/> instance for the text content using <paramref name="source"/> argument.
        /// </summary>
        /// <param name="source">The source of the content.</param>
        /// <returns>The <see cref="DataProvider{TData, TOwner}"/> instance for the text content.</returns>
        public DataProvider<string, TOwner> GetContent(ContentSource source)
        {
            return GetOrCreateDataProvider($"content ({source.ToString(TermCase.MidSentence)})", () => ContentExtractor.Get(this, source));
        }

        /// <summary>
        /// Gets the data provider by name or creates and stores a new instance with the specified <paramref name="providerName"/> and using <paramref name="valueGetFunction"/>.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="providerName">Name of the provider.</param>
        /// <param name="valueGetFunction">The function that gets a value.</param>
        /// <returns>A new instance of <see cref="DataProvider{TData, TOwner}"/> type or already stored one.</returns>
        protected internal DataProvider<TValue, TOwner> GetOrCreateDataProvider<TValue>(string providerName, Func<TValue> valueGetFunction)
        {
            if (dataProviders.TryGetValue(providerName, out object dataProviderAsObject) && dataProviderAsObject is DataProvider<TValue, TOwner> dataProvider)
                return dataProvider;

            return CreateDataProvider(providerName, valueGetFunction);
        }

        DataProvider<TValue, TOwner> IUIComponent<TOwner>.GetOrCreateDataProvider<TValue>(string providerName, Func<TValue> valueGetFunction) =>
            GetOrCreateDataProvider(providerName, valueGetFunction);

        /// <summary>
        /// Creates and stores a new instance with the specified <paramref name="providerName"/> and using <paramref name="valueGetFunction"/>.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="providerName">Name of the provider.</param>
        /// <param name="valueGetFunction">The function that gets a value.</param>
        /// <returns>A new instance of <see cref="DataProvider{TData, TOwner}"/> type.</returns>
        protected internal DataProvider<TValue, TOwner> CreateDataProvider<TValue>(string providerName, Func<TValue> valueGetFunction)
        {
            var dataProvider = new DataProvider<TValue, TOwner>(this, valueGetFunction, providerName);
            dataProviders[providerName] = dataProvider;
            return dataProvider;
        }

        /// <summary>
        /// Executes the triggers.
        /// </summary>
        /// <param name="on">The event to trigger.</param>
        protected void ExecuteTriggers(TriggerEvents on)
        {
            Triggers.Execute(on);
        }

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

        /// <summary>
        /// Cleans up the current instance.
        /// </summary>
        protected internal override void CleanUp()
        {
            foreach (var control in Controls)
                control.CleanUp();

            Controls.Clear();
        }
    }
}
