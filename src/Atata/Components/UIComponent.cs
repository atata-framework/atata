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
            Driver = AtataContext.Current.Driver;
            Log = AtataContext.Current.Log;
        }

        /// <summary>
        /// Gets the owner component.
        /// </summary>
        public UIComponent Owner { get; internal set; }

        /// <summary>
        /// Gets the parent component.
        /// </summary>
        public UIComponent Parent { get; internal set; }

        protected internal ILogManager Log { get; internal set; }

        protected internal RemoteWebDriver Driver { get; internal set; }

        /// <summary>
        /// Gets the source of the scope.
        /// </summary>
        public ScopeSource ScopeSource { get; internal set; }

        protected internal IScopeLocator ScopeLocator { get; internal set; }

        protected internal bool CacheScopeElement { get; set; }

        /// <summary>
        /// Gets the name of the component.
        /// </summary>
        public string ComponentName { get; internal set; }

        /// <summary>
        /// Gets the name of the component type.
        /// Returns the value of <see cref="UIComponentDefinitionAttribute.ComponentTypeName"/> property for control from <see cref="ControlDefinitionAttribute"/>
        /// and for page object from <see cref="PageObjectDefinitionAttribute"/>.
        /// </summary>
        public string ComponentTypeName { get; internal set; }

        /// <summary>
        /// Gets the full name of the component including parent component full name, own component name and own component type name.
        /// </summary>
        public string ComponentFullName
        {
            get { return BuildComponentFullName(); }
        }

        /// <summary>
        /// Gets the metadata of the component.
        /// </summary>
        public UIComponentMetadata Metadata { get; internal set; }

        /// <summary>
        /// Gets the <see cref="IWebElement"/> instance that represents the scope HTML element.
        /// Also executes <see cref="TriggerEvents.BeforeAccess" /> and <see cref="TriggerEvents.AfterAccess" /> triggers.
        /// </summary>
        public IWebElement Scope =>
            CacheScopeElement && cachedScope != null
                ? cachedScope
                : GetScopeElement();

        /// <summary>
        /// Gets the <see cref="IWebElement"/> instance that represents the scope HTML element.
        /// Also executes <see cref="TriggerEvents.BeforeAccess" /> and <see cref="TriggerEvents.AfterAccess" /> triggers.
        /// </summary>
        /// <param name="options">
        /// The search options.
        /// If set to <see langword="null"/>, then it uses <c>SearchOptions.Safely()</c>.</param>
        /// <returns>The <see cref="IWebElement"/> instance of the scope.</returns>
        public IWebElement GetScope(SearchOptions options = null)
        {
            return GetScopeElement(options ?? SearchOptions.Safely());
        }

        protected IWebElement GetScopeElement(SearchOptions searchOptions = null)
        {
            if (ScopeLocator == null)
                throw new InvalidOperationException($"{nameof(ScopeLocator)} is missing.");

            SearchOptions actualSearchOptions = searchOptions ?? new SearchOptions();

            var cache = AtataContext.Current.UIComponentScopeCache;
            bool isActivatedAccessChainCache = cache.AcquireActivationOfAccessChain();

            IWebElement element;

            try
            {
                if (!cache.TryGet(this, actualSearchOptions.Visibility, out element))
                    element = OnGetScopeElement(actualSearchOptions);

                if (!isActivatedAccessChainCache)
                    cache.AddToAccessChain(this, actualSearchOptions.Visibility, element);
            }
            finally
            {
                if (isActivatedAccessChainCache)
                    cache.ReleaseAccessChain();
            }

            if (CacheScopeElement)
                cachedScope = element;

            return element;
        }

        internal abstract IWebElement OnGetScopeElement(SearchOptions searchOptions);

        protected internal virtual void ApplyMetadata(UIComponentMetadata metadata)
        {
        }

        /// <summary>
        /// Builds the full name of the component including parent component full name, own component name and own component type name.
        /// </summary>
        /// <returns>The full name of the component.</returns>
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
        /// <param name="options">
        /// The search options.
        /// If set to <see langword="null"/>, then it uses <c>SearchOptions.Safely()</c>.</param>
        /// <returns><see langword="true"/> if the component exists; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="NoSuchElementException">The <paramref name="options"/> has <see cref="SearchOptions.IsSafely"/> property equal to <see langword="false"/> value and the component doesn't exist.</exception>
        public bool Exists(SearchOptions options = null)
        {
            return GetScopeElement(options ?? SearchOptions.Safely()) != null;
        }

        /// <summary>
        /// Determines whether the component is missing.
        /// </summary>
        /// <param name="options">
        /// The search options.
        /// If set to <see langword="null"/>, then it uses <c>SearchOptions.Safely()</c>.</param>
        /// <returns><see langword="true"/> if the component is missing; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="NotMissingElementException">The <paramref name="options"/> has <see cref="SearchOptions.IsSafely"/> property equal to <see langword="false"/> value and the component exists.</exception>
        public bool Missing(SearchOptions options = null)
        {
            return OnMissing(options ?? SearchOptions.Safely());
        }

        internal virtual bool OnMissing(SearchOptions options)
        {
            return ScopeLocator.IsMissing(options);
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance including <see cref="ComponentFullName"/> and <see cref="Scope"/> element details.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder(ComponentFullName);
            IWebElement scope = GetScopeElement(SearchOptions.SafelyAtOnce());

            if (scope != null)
                builder.AppendLine().Append(scope.ToDetailedString());

            return builder.ToString();
        }

        /// <summary>
        /// Cleans up the current instance.
        /// </summary>
        protected internal abstract void CleanUp();
    }
}
