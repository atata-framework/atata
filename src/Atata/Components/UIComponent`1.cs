using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;

namespace Atata
{
    public abstract class UIComponent<TOwner> : UIComponent, IUIComponent<TOwner>
        where TOwner : PageObject<TOwner>
    {
        private readonly Dictionary<string, object> dataProviders = new Dictionary<string, object>();

        protected UIComponent()
        {
            Controls = new UIComponentChildrenList<TOwner>(this);
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
        /// Gets the text content.
        /// </summary>
        public DataProvider<string, TOwner> Content => GetOrCreateDataProvider(nameof(Content).ToString(TermCase.Lower), GetContent);

        public UIComponentVerificationProvider<UIComponent<TOwner>, TOwner> Should => new UIComponentVerificationProvider<UIComponent<TOwner>, TOwner>(this);

        TOwner IUIComponent<TOwner>.Owner => Owner;

        IUIComponent<TOwner> IUIComponent<TOwner>.Parent => Parent;

        IWebElement IUIComponent<TOwner>.Scope => Scope;

        IScopeLocator IUIComponent<TOwner>.ScopeLocator => ScopeLocator;

        string IUIComponent<TOwner>.ComponentName => ComponentName;

        string IUIComponent<TOwner>.ComponentTypeName => ComponentTypeName;

        string IUIComponent<TOwner>.ComponentFullName => ComponentFullName;

        protected internal UIComponentChildrenList<TOwner> Controls { get; private set; }

        UIComponentChildrenList<TOwner> IUIComponent<TOwner>.Controls => Controls;

        UIComponentMetadata IUIComponent<TOwner>.Metadata => Metadata;

        ScopeSource IUIComponent<TOwner>.ScopeSource => ScopeSource;

        protected internal virtual void InitComponent()
        {
            UIComponentResolver.Resolve<TOwner>(this);
        }

        private string GetContent()
        {
            return Scope.Text;
        }

        protected internal DataProvider<TValue, TOwner> GetOrCreateDataProvider<TValue>(string providerName, Func<TValue> valueGetFunction)
        {
            object dataProvider;
            if (dataProviders.TryGetValue(providerName, out dataProvider))
                return (DataProvider<TValue, TOwner>)dataProvider;
            else
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
            if (Triggers == null || Triggers.Length == 0 || on == TriggerEvents.None)
                return;

            TriggerContext<TOwner> context = new TriggerContext<TOwner>
            {
                Event = on,
                Driver = Driver,
                Log = Log,
                Component = this
            };

            var triggers = Triggers.Where(x => x.On.HasFlag(on));

            foreach (var trigger in triggers)
                trigger.Execute(context);

            if (on == TriggerEvents.OnPageObjectInit || on == TriggerEvents.OnPageObjectLeave)
            {
                foreach (UIComponent<TOwner> child in Children.Cast<UIComponent<TOwner>>())
                {
                    child.ExecuteTriggers(on);
                }
            }
        }

        protected internal override void CleanUp()
        {
            foreach (var item in Children)
                item.CleanUp();

            Children.Clear();
        }
    }
}
