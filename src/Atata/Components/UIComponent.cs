using System;
using System.Collections.Generic;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace Atata
{
    public abstract class UIComponent
    {
        private IWebElement cachedScope;

        protected UIComponent()
        {
            Children = new List<UIComponent>();
        }

        protected internal UIComponent Owner { get; internal set; }
        protected internal UIComponent Parent { get; internal set; }
        protected internal List<UIComponent> Children { get; private set; }

        protected internal ILogManager Log { get; internal set; }
        protected internal RemoteWebDriver Driver { get; internal set; }

        protected internal ScopeSource ScopeSource { get; internal set; }
        protected internal IScopeLocator ScopeLocator { get; internal set; }
        protected internal bool CacheScopeElement { get; set; }
        protected internal string ComponentName { get; internal set; }
        protected internal string ComponentTypeName { get; internal set; }

        protected internal string ComponentFullName
        {
            get { return string.Format("\"{0}\" {1}", ComponentName, ComponentTypeName ?? "component"); }
        }

        protected internal UIComponentMetadata Metadata { get; internal set; }

        protected internal TriggerAttribute[] Triggers { get; internal set; }

        protected internal IWebElement Scope
        {
            get
            {
                if (CacheScopeElement && cachedScope != null)
                    return cachedScope;
                else
                    return GetScopeElement();
            }
        }

        protected IWebElement GetScopeElement(SearchOptions searchOptions = null)
        {
            if (ScopeLocator == null)
                throw new InvalidOperationException("ScopeLocator is missing.");

            searchOptions = searchOptions ?? SearchOptions.Safely(false);

            IWebElement element = ScopeLocator.GetElement(searchOptions);
            if (!searchOptions.IsSafely && element == null)
                throw ExceptionFactory.CreateForNoSuchElement(ComponentFullName);

            if (CacheScopeElement)
                cachedScope = element;

            return element;
        }

        protected internal virtual void ApplyMetadata(UIComponentMetadata metadata)
        {
        }

        public bool Exists(SearchOptions options = null)
        {
            return GetScopeElement(options ?? SearchOptions.Safely()) != null;
        }

        public bool Missing(SearchOptions options = null)
        {
            return ScopeLocator.IsMissing(options ?? SearchOptions.Safely());
        }

        protected void Wait(double seconds)
        {
            Thread.Sleep((int)(seconds * 1000));
        }
    }
}
