using System;
using System.Collections.Generic;
using OpenQA.Selenium;

namespace Atata
{
    /// <summary>
    /// Represents the base class for UI components.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    public abstract class UIComponent<TOwner> : UIComponent, IUIComponent<TOwner>
        where TOwner : PageObject<TOwner>
    {
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

        protected internal new TOwner Owner
        {
            get { return (TOwner)base.Owner; }
            internal set { base.Owner = value; }
        }

        protected internal new UIComponent<TOwner> Parent
        {
            get { return (UIComponent<TOwner>)base.Parent; }
            internal set { base.Parent = value; }
        }

        /// <summary>
        /// Gets the <see cref="DataProvider{TData, TOwner}"/> instance for the value indicating whether the component is visible.
        /// </summary>
        public DataProvider<bool, TOwner> IsVisible => GetOrCreateDataProvider("visible", GetIsVisible);

        /// <summary>
        /// Gets the <see cref="DataProvider{TData, TOwner}"/> instance for the text content. Gets content using <see cref="Atata.ContentSourceAttribute"/> or, by default, uses Text property of component scope <see cref="IWebElement"/> element.
        /// </summary>
        public DataProvider<string, TOwner> Content => GetOrCreateDataProvider(nameof(Content).ToString(TermCase.Lower), GetContent);

        /// <summary>
        /// Gets the verification provider that gives a set of verification extension methods.
        /// </summary>
        public UIComponentVerificationProvider<UIComponent<TOwner>, TOwner> Should => new UIComponentVerificationProvider<UIComponent<TOwner>, TOwner>(this);

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

        TOwner IUIComponent<TOwner>.Owner => Owner;

        IUIComponent<TOwner> IUIComponent<TOwner>.Parent => Parent;

        IScopeLocator IUIComponent<TOwner>.ScopeLocator => ScopeLocator;

        string IUIComponent<TOwner>.ComponentName => ComponentName;

        string IUIComponent<TOwner>.ComponentTypeName => ComponentTypeName;

        string IUIComponent<TOwner>.ComponentFullName => ComponentFullName;

        protected internal UIComponentChildrenList<TOwner> Controls { get; private set; }

        UIComponentChildrenList<TOwner> IUIComponent<TOwner>.Controls => Controls;

        UIComponentMetadata IUIComponent<TOwner>.Metadata => Metadata;

        ScopeSource IUIComponent<TOwner>.ScopeSource => ScopeSource;

        /// <summary>
        /// Gets the set of triggers. Provides the functionality to get/add/remove triggers dynamically.
        /// </summary>
        public UIComponentTriggerSet<TOwner> Triggers { get; internal set; }

        /// <summary>
        /// Gets the instance of <see cref="Atata.ContentSourceAttribute"/> or null, if not found.
        /// </summary>
        protected ContentSourceAttribute ContentSourceAttribute => Metadata.Get<ContentSourceAttribute>(AttributeLevels.All);

        protected internal virtual void InitComponent()
        {
            UIComponentResolver.Resolve(this);
        }

        internal sealed override IWebElement OnGetScopeElement(SearchOptions searchOptions)
        {
            ExecuteTriggers(TriggerEvents.BeforeAccess);

            IWebElement element = ScopeLocator.GetElement(searchOptions);
            if (!searchOptions.IsSafely && element == null)
                throw ExceptionFactory.CreateForNoSuchElement(ComponentFullName);

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

        protected virtual bool GetIsVisible()
        {
            return Scope.Displayed;
        }

        protected virtual string GetContent()
        {
            var contentSourceAttribute = ContentSourceAttribute;

            return contentSourceAttribute != null
                ? contentSourceAttribute.GetContent(Scope)
                : Scope.Text;
        }

        protected internal DataProvider<TValue, TOwner> GetOrCreateDataProvider<TValue>(string providerName, Func<TValue> valueGetFunction)
        {
            if (dataProviders.TryGetValue(providerName, out object dataProviderAsObject) && dataProviderAsObject is DataProvider<TValue, TOwner> dataProvider)
                return dataProvider;

            return CreateDataProvider(providerName, valueGetFunction);
        }

        DataProvider<TValue, TOwner> IUIComponent<TOwner>.GetOrCreateDataProvider<TValue>(string providerName, Func<TValue> valueGetFunction) => GetOrCreateDataProvider(providerName, valueGetFunction);

        protected internal DataProvider<TValue, TOwner> CreateDataProvider<TValue>(string providerName, Func<TValue> valueGetFunction)
        {
            var dataProvider = new DataProvider<TValue, TOwner>(this, valueGetFunction, providerName);
            dataProviders[providerName] = dataProvider;
            return dataProvider;
        }

        protected void ExecuteTriggers(TriggerEvents on)
        {
            Triggers.Execute(on);
        }

        protected internal TComponentToFind GetAncestor<TComponentToFind>()
            where TComponentToFind : UIComponent<TOwner>
        {
            return Parent is TComponentToFind ?
                (TComponentToFind)Parent :
                Parent?.GetAncestor<TComponentToFind>();
        }

        TComponentToFind IUIComponent<TOwner>.GetAncestor<TComponentToFind>() => GetAncestor<TComponentToFind>();

        protected internal TComponentToFind GetAncestorOrSelf<TComponentToFind>()
            where TComponentToFind : UIComponent<TOwner>
        {
            return this is TComponentToFind ?
                (TComponentToFind)this :
                Parent?.GetAncestorOrSelf<TComponentToFind>();
        }

        TComponentToFind IUIComponent<TOwner>.GetAncestorOrSelf<TComponentToFind>() => GetAncestorOrSelf<TComponentToFind>();

        protected internal override void CleanUp()
        {
            foreach (var control in Controls)
                control.CleanUp();

            Controls.Clear();
        }
    }
}
