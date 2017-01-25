using System;
using System.Text;
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
        }

        protected internal UIComponent Owner { get; internal set; }

        /// <summary>
        /// Gets the parent component.
        /// </summary>
        protected internal UIComponent Parent { get; internal set; }

        protected internal ILogManager Log { get; internal set; }

        protected internal RemoteWebDriver Driver { get; internal set; }

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
                throw new InvalidOperationException($"{nameof(ScopeLocator)} is missing.");

            IWebElement element = OnGetScopeElement(searchOptions ?? new SearchOptions());

            if (CacheScopeElement)
                cachedScope = element;

            return element;
        }

        internal abstract IWebElement OnGetScopeElement(SearchOptions searchOptions);

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

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder(ComponentFullName);
            IWebElement scope = GetScopeElement(SearchOptions.SafelyAtOnce());

            if (scope != null)
                builder.AppendLine().Append(scope.ToString());

            return builder.ToString();
        }

        protected internal abstract void CleanUp();
    }
}
