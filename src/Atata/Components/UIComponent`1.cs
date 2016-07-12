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

        public UIComponentDataProvider<string, TOwner> Content => GetOrCreateDataProvider(nameof(Content).ToString(TermCase.Lower), GetContent);

        IPageObject<TOwner> IUIComponent<TOwner>.Owner => Owner;

        IUIComponent<TOwner> IUIComponent<TOwner>.Parent => Parent;

        IWebElement IUIComponent<TOwner>.Scope => Scope;

        IScopeLocator IUIComponent<TOwner>.ScopeLocator => ScopeLocator;

        string IUIComponent<TOwner>.ComponentName => ComponentName;

        string IUIComponent<TOwner>.ComponentTypeName => ComponentTypeName;

        string IUIComponent<TOwner>.ComponentFullName => ComponentFullName;

        ScopeSource IUIComponent<TOwner>.ScopeSource => ScopeSource;

        protected internal virtual void InitComponent()
        {
            UIComponentResolver.Resolve<TOwner>(this);
        }

        private string GetContent()
        {
            return Scope.Text;
        }

        public TOwner VerifyExists()
        {
            Log.StartVerificationSection("{0} exists", ComponentFullName);
            GetScopeElement();
            Log.EndSection();

            return Owner;
        }

        public TOwner VerifyMissing()
        {
            Log.StartVerificationSection("{0} is missing", ComponentFullName);
            ScopeLocator.IsMissing();
            Log.EndSection();

            return Owner;
        }

        protected internal TControl CreateControl<TControl>(string name, params Attribute[] attributes)
            where TControl : Control<TOwner>
        {
            return UIComponentResolver.CreateComponent<TControl, TOwner>(this, name, attributes);
        }

        protected internal ClickableControl<TOwner> CreateClickable(string name, params Attribute[] attributes)
        {
            return CreateControl<ClickableControl<TOwner>>(name, attributes);
        }

        protected internal ClickableControl<TNavigateTo, TOwner> CreateClickable<TNavigateTo>(string name, params Attribute[] attributes)
            where TNavigateTo : PageObject<TNavigateTo>
        {
            return CreateControl<ClickableControl<TNavigateTo, TOwner>>(name, attributes);
        }

        protected internal ClickableControl<TNavigateTo, TOwner> CreateClickable<TNavigateTo>(string name, Func<TNavigateTo> navigationPageObjectCreator, params Attribute[] attributes)
            where TNavigateTo : PageObject<TNavigateTo>
        {
            var control = CreateControl<ClickableControl<TNavigateTo, TOwner>>(name, attributes);
            control.NavigationPageObjectCreator = navigationPageObjectCreator;
            return control;
        }

        protected internal LinkControl<TOwner> CreateLink(string name, params Attribute[] attributes)
        {
            return CreateControl<LinkControl<TOwner>>(name, attributes);
        }

        protected internal LinkControl<TNavigateTo, TOwner> CreateLink<TNavigateTo>(string name, params Attribute[] attributes)
            where TNavigateTo : PageObject<TNavigateTo>
        {
            return CreateControl<LinkControl<TNavigateTo, TOwner>>(name, attributes);
        }

        protected internal LinkControl<TNavigateTo, TOwner> CreateLink<TNavigateTo>(string name, Func<TNavigateTo> navigationPageObjectCreator, params Attribute[] attributes)
            where TNavigateTo : PageObject<TNavigateTo>
        {
            var control = CreateControl<LinkControl<TNavigateTo, TOwner>>(name, attributes);
            control.NavigationPageObjectCreator = navigationPageObjectCreator;
            return control;
        }

        protected internal ButtonControl<TOwner> CreateButton(string name, params Attribute[] attributes)
        {
            return CreateControl<ButtonControl<TOwner>>(name, attributes);
        }

        protected internal ButtonControl<TNavigateTo, TOwner> CreateButton<TNavigateTo>(string name, params Attribute[] attributes)
            where TNavigateTo : PageObject<TNavigateTo>
        {
            return CreateControl<ButtonControl<TNavigateTo, TOwner>>(name, attributes);
        }

        protected internal ButtonControl<TNavigateTo, TOwner> CreateButton<TNavigateTo>(string name, Func<TNavigateTo> navigationPageObjectCreator, params Attribute[] attributes)
            where TNavigateTo : PageObject<TNavigateTo>
        {
            var control = CreateControl<ButtonControl<TNavigateTo, TOwner>>(name, attributes);
            control.NavigationPageObjectCreator = navigationPageObjectCreator;
            return control;
        }

        protected UIComponentDataProvider<TValue, TOwner> GetOrCreateDataProvider<TValue>(string providerName, Func<TValue> valueGetFunction)
        {
            object dataProvider;
            if (dataProviders.TryGetValue(providerName, out dataProvider))
                return (UIComponentDataProvider<TValue, TOwner>)dataProvider;
            else
                return CreateDataProvider(providerName, valueGetFunction);
        }

        protected UIComponentDataProvider<TValue, TOwner> CreateDataProvider<TValue>(string providerName, Func<TValue> valueGetFunction)
        {
            var dataProvider = new UIComponentDataProvider<TValue, TOwner>(this, valueGetFunction, providerName);
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

        TControl IUIComponent<TOwner>.CreateControl<TControl>(string name, params Attribute[] attributes)
        {
            return CreateControl<TControl>(name, attributes);
        }
    }
}
