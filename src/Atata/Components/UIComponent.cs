using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace Atata
{
    /// <summary>
    /// Represents the base class for UI components.
    /// </summary>
    public abstract class UIComponent
    {
        private IWebElement cachedScope;

        protected UIComponent()
        {
            Children = new List<UIComponent>();

            Attributes = new UIComponentAttributeProvider(this);
            Css = new UIComponentCssProvider(this);
        }

        protected internal UIComponent Owner { get; internal set; }

        /// <summary>
        /// Gets the parent component.
        /// </summary>
        protected internal UIComponent Parent { get; internal set; }

        protected internal List<UIComponent> Children { get; private set; }

        protected internal ILogManager Log { get; internal set; }

        protected internal RemoteWebDriver Driver { get; internal set; }

        /// <summary>
        /// Gets the scope element's attributes.
        /// </summary>
        public UIComponentAttributeProvider Attributes { get; private set; }

        /// <summary>
        /// Gets the scope element's CSS properties.
        /// </summary>
        public UIComponentCssProvider Css { get; private set; }

        protected internal ScopeSource ScopeSource { get; internal set; }

        protected internal IScopeLocator ScopeLocator { get; internal set; }

        protected internal bool CacheScopeElement { get; set; }

        protected internal string ComponentName { get; internal set; }

        protected internal string ComponentTypeName { get; internal set; }

        protected internal string ComponentFullName
        {
            get { return BuildComponentFullName(); }
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

            searchOptions = searchOptions ?? SearchOptions.Unsafely();

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

        protected virtual string BuildComponentFullName()
        {
            StringBuilder builder = new StringBuilder();

            if (Parent != null && !Parent.GetType().IsSubclassOfRawGeneric(typeof(PageObject<>)))
            {
                string parentFullName = Parent.ComponentFullName;
                builder.
                    Append(parentFullName).
                    Append(parentFullName.EndsWith("s") ? "'" : "'s").
                    Append(" ");
            }

            builder.AppendFormat("\"{0}\" {1}", ComponentName, ComponentTypeName ?? "component");
            return builder.ToString();
        }

        /// <summary>
        /// Determines whether the component exists.
        /// </summary>
        /// <param name="options">The options. If set to null, then it uses <c>SearchOptions.Safely()</c>.</param>
        /// <returns>true if the component exists; otherwise, false.</returns>
        /// <exception cref="NoSuchElementException">The <paramref name="options"/> has IsSafely property equal to false value and the component doesn't exist.</exception>
        public bool Exists(SearchOptions options = null)
        {
            return GetScopeElement(options ?? SearchOptions.Safely()) != null;
        }

        /// <summary>
        /// Determines whether the component is missing.
        /// </summary>
        /// <param name="options">The options. If set to null, then it uses <c>SearchOptions.Safely()</c>.</param>
        /// <returns>true if the component is missing; otherwise, false.</returns>
        /// <exception cref="NotMissingElementException">The <paramref name="options"/> has IsSafely property equal to false value and the component exists.</exception>
        public bool Missing(SearchOptions options = null)
        {
            return ScopeLocator.IsMissing(options ?? SearchOptions.Safely());
        }

        protected void Wait(double seconds)
        {
            Thread.Sleep((int)(seconds * 1000));
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder(ComponentFullName);
            IWebElement scope = GetScopeElement(SearchOptions.SafelyAndImmediately());

            if (scope != null)
                builder.AppendLine().Append(scope.ToString());

            return builder.ToString();
        }

        protected internal abstract void CleanUp();
    }
}
