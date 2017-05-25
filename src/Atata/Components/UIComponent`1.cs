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
            Attributes = new UIComponentAttributeProvider<TOwner>() { Component = this };
            Css = new UIComponentCssProvider<TOwner>() { Component = this };
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
        /// Gets the <see cref="DataProvider{TData, TOwner}"/> instance for the value indicating whether the control is visible.
        /// </summary>
        public DataProvider<bool, TOwner> IsVisible => GetOrCreateDataProvider("visible", GetIsVisible);

        /// <summary>
        /// Gets the <see cref="DataProvider{TData, TOwner}"/> instance for the text content.
        /// </summary>
        public DataProvider<string, TOwner> Content => GetOrCreateDataProvider(nameof(Content).ToString(TermCase.Lower), GetContent);

        /// <summary>
        /// Gets the verification provider that gives a set of verification extension methods.
        /// </summary>
        public UIComponentVerificationProvider<UIComponent<TOwner>, TOwner> Should => new UIComponentVerificationProvider<UIComponent<TOwner>, TOwner>(this);

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
            return Scope.Text;
        }

        protected internal DataProvider<TValue, TOwner> GetOrCreateDataProvider<TValue>(string providerName, Func<TValue> valueGetFunction)
        {
            object dataProvider;
            if (dataProviders.TryGetValue(providerName, out dataProvider) && dataProvider is DataProvider<TValue, TOwner>)
                return (DataProvider<TValue, TOwner>)dataProvider;

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
                Parent != null ?
                    Parent.GetAncestor<TComponentToFind>() :
                    null;
        }

        TComponentToFind IUIComponent<TOwner>.GetAncestor<TComponentToFind>() => GetAncestor<TComponentToFind>();

        protected internal TComponentToFind GetAncestorOrSelf<TComponentToFind>()
            where TComponentToFind : UIComponent<TOwner>
        {
            return this is TComponentToFind ?
                (TComponentToFind)this :
                Parent != null ?
                    Parent.GetAncestorOrSelf<TComponentToFind>() :
                    null;
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
