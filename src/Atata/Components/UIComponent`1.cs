using System;
using System.Linq;
using OpenQA.Selenium;

namespace Atata
{
    public abstract class UIComponent<TOwner> : UIComponent, IUIComponent<TOwner>
        where TOwner : PageObject<TOwner>
    {
        protected UIComponent()
        {
            Content = new UIComponentContentValueProvider<TOwner>(this);
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

        public UIComponentContentValueProvider<TOwner> Content { get; private set; }

        IPageObject<TOwner> IUIComponent<TOwner>.Owner => Owner;

        IUIComponent<TOwner> IUIComponent<TOwner>.Parent => Parent;

        IWebElement IUIComponent<TOwner>.Scope => Scope;

        IScopeLocator IUIComponent<TOwner>.ScopeLocator => ScopeLocator;

        string IUIComponent<TOwner>.ComponentName => ComponentName;

        string IUIComponent<TOwner>.ComponentTypeName => ComponentTypeName;

        ScopeSource IUIComponent<TOwner>.ScopeSource => ScopeSource;

        protected internal virtual void InitComponent()
        {
            UIComponentResolver.Resolve<TOwner>(this);
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

        protected UIComponentValueProvider<TValue, TOwner> CreateValueProvider<TValue>(Func<TValue> valueGetFunction, string providerName)
        {
            return new UIComponentValueProvider<TValue, TOwner>(this, valueGetFunction, providerName);
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
                foreach (UIComponent<TOwner> child in Children)
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
