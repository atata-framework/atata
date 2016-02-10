using Humanizer.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;
using System.Linq;

namespace Atata
{
    public abstract class UIComponent
    {
        private IWebElement scope;

        protected UIComponent()
        {
        }

        static UIComponent()
        {
            SetupHumanizerSettings();
        }

        protected internal UIComponent Owner { get; internal set; }
        protected internal UIComponent Parent { get; internal set; }

        protected internal ILogManager Log { get; internal set; }
        protected internal RemoteWebDriver Driver { get; internal set; }

        protected internal ScopeSource ScopeSource { get; internal set; }
        protected internal IScopeLocator ScopeLocator { get; internal set; }
        protected internal bool CacheScopeElement { get; internal set; }
        protected internal string ComponentName { get; internal set; }

        protected internal TriggerAttribute[] Triggers { get; internal set; }

        protected internal IWebElement Scope
        {
            get
            {
                if (CacheScopeElement && scope != null)
                    return scope;
                else
                    return GetScopeElement();
            }
        }

        private static void SetupHumanizerSettings()
        {
            Configurator.EnumDescriptionPropertyLocator = p => p.Name == "Description" || p.Name == "Value";
        }

        protected IWebElement GetScopeElement(SearchOptions searchOptions = null)
        {
            if (ScopeLocator == null)
                throw new InvalidOperationException("ScopeLocator is missing");

            searchOptions = searchOptions ?? SearchOptions.Safely(false);

            IWebElement element = ScopeLocator.GetElement(searchOptions);
            if (!searchOptions.IsSafely && element == null)
                throw ExceptionsFactory.CreateForNoSuchElement(ComponentName);

            if (CacheScopeElement)
                this.scope = element;

            return element;
        }

        protected internal virtual void ApplyMetadata(UIComponentMetadata metadata)
        {
        }

        public bool Exists()
        {
            return ScopeLocator.GetElement() != null;
        }

        public bool Missing()
        {
            return ScopeLocator.GetElement() == null;
        }

        protected void RunTriggers(TriggerEvents on)
        {
            if (Triggers == null || on == TriggerEvents.None)
                return;

            var triggers = Triggers.Where(x => x.On.HasFlag(on));

            TriggerContext context = new TriggerContext
            {
                Driver = Driver,
                Log = Log,
                Component = this,
                ParentComponent = Parent,
                ComponentScopeLocator = ScopeLocator,
                ParentComponentScopeLocator = Parent != null ? Parent.ScopeLocator : null
            };

            foreach (var trigger in triggers)
                trigger.Run(context);
        }
    }
}
