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
        protected UIComponent()
        {
        }

        /// <summary>
        /// Gets the <see cref="AtataContext"/> instance with which this component is associated.
        /// </summary>
        public abstract AtataContext Context { get; }

        /// <summary>
        /// Gets the owner component.
        /// </summary>
        public UIComponent Owner { get; internal set; }

        /// <summary>
        /// Gets the parent component.
        /// </summary>
        public UIComponent Parent { get; internal set; }

        protected internal ILogManager Log => Context.Log;

        protected internal RemoteWebDriver Driver => Context.Driver;

        /// <summary>
        /// Gets the source of the scope.
        /// </summary>
        public abstract ScopeSource ScopeSource { get; }

        protected internal IScopeLocator ScopeLocator { get; internal set; }

        [Obsolete("Use " + nameof(UsesScopeCache) + " instead.")] // Obsolete since v1.13.0.
        protected internal bool CacheScopeElement
        {
            get => UsesScopeCache;
            set
            {
                if (value)
                {
                    if (Metadata.Get<UsesScopeCacheAttribute>(x => x.At(AttributeLevels.Declared).Where(a => a.UsesCache)) == null)
                        Metadata.Push(new UsesScopeCacheAttribute());
                }
                else
                {
                    Metadata.RemoveAll(x => x is UsesScopeCacheAttribute);
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether to use scope cache.
        /// Returns a <see cref="ICanUseCache.UsesCache"/> value of an associated with the component
        /// <see cref="UsesScopeCacheAttribute"/> or <see cref="UsesCacheAttribute"/>.
        /// Returns <see langword="false"/>, as by default, when the attribute is not associated.
        /// </summary>
        protected bool UsesScopeCache =>
            Metadata.Get<ICanUseCache>(filter => filter.Where(x => x is UsesCacheAttribute || x is UsesScopeCacheAttribute))
                ?.UsesCache ?? false;

        /// <summary>
        /// Gets or sets the name of the component.
        /// </summary>
        public string ComponentName { get; set; }

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
        /// Gets the <see cref="IWebElement"/> instance that represents the scope HTML element associated with this component.
        /// Also executes <see cref="TriggerEvents.BeforeAccess" /> and <see cref="TriggerEvents.AfterAccess" /> triggers.
        /// </summary>
        /// <exception cref="NoSuchElementException">Element not found.</exception>
        public IWebElement Scope =>
            GetScopeElement();

        /// <summary>
        /// Gets or sets the cached scope element.
        /// </summary>
        protected IWebElement CachedScope { get; set; }

        /// <summary>
        /// Gets the <see cref="ISearchContext"/> instance that represents the scope search context
        /// (where to find the children of this component).
        /// By default is the same as <see cref="Scope"/>.
        /// Also can execute <see cref="TriggerEvents.BeforeAccess" /> and <see cref="TriggerEvents.AfterAccess" /> triggers.
        /// </summary>
        public ISearchContext ScopeContext =>
            OnGetScopeContext(new SearchOptions());

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

        /// <summary>
        /// Gets the <see cref="ISearchContext"/> instance that represents the scope search context
        /// (where to find the children of this component).
        /// Also can execute <see cref="TriggerEvents.BeforeAccess" /> and <see cref="TriggerEvents.AfterAccess" /> triggers.
        /// </summary>
        /// <param name="searchOptions">
        /// The search options.
        /// If set to <see langword="null"/>, then it uses <c>SearchOptions.Safely()</c>.</param>
        /// <returns>The <see cref="ISearchContext"/> instance of the scope context.</returns>
        public ISearchContext GetScopeContext(SearchOptions searchOptions = null)
        {
            return OnGetScopeContext(searchOptions ?? SearchOptions.Safely());
        }

        protected virtual ISearchContext OnGetScopeContext(SearchOptions searchOptions)
        {
            return ShouldUseParentScope()
                ? Parent.GetScopeContext(searchOptions)
                : GetScopeElement(searchOptions);
        }

        protected IWebElement GetScopeElement(SearchOptions searchOptions = null)
        {
            if (CachedScope != null && UsesScopeCache)
                return CachedScope;

            if (ShouldUseParentScope())
                return Parent.GetScopeElement(searchOptions);

            if (ScopeLocator == null)
                throw new InvalidOperationException($"{nameof(ScopeLocator)} is missing.");

            SearchOptions actualSearchOptions = searchOptions ?? new SearchOptions();

            var cache = Context.UIComponentScopeCache;
            bool isActivatedAccessChainCache = cache.AcquireActivationOfAccessChain();

            IWebElement element;

            try
            {
                if (!cache.TryGet(this, actualSearchOptions.Visibility, out element))
                    element = OnGetScopeElement(actualSearchOptions);

                if (!isActivatedAccessChainCache && element != null)
                    cache.AddToAccessChain(this, actualSearchOptions.Visibility, element);
            }
            finally
            {
                if (isActivatedAccessChainCache)
                    cache.ReleaseAccessChain();
            }

            if (UsesScopeCache)
                CachedScope = element;

            return element;
        }

        internal abstract IWebElement OnGetScopeElement(SearchOptions searchOptions);

        private bool ShouldUseParentScope()
        {
            if (!(ScopeLocator is StrategyScopeLocator))
                return false;

            FindAttribute findAttribute = Metadata.ResolveFindAttribute();

            return findAttribute is UseParentScopeAttribute;
        }

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
                    Append(parentFullName[parentFullName.Length - 1] == 's' ? "'" : "'s").
                    Append(' ');
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
        /// <exception cref="NoSuchElementException">
        /// The <paramref name="options"/> has <see cref="SearchOptions.IsSafely"/> property
        /// equal to <see langword="false"/> value and the component doesn't exist.
        /// </exception>
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
        /// <exception cref="NotMissingElementException">
        /// The <paramref name="options"/> has <see cref="SearchOptions.IsSafely"/> property
        /// equal to <see langword="false"/> value and the component exists.
        /// </exception>
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

        /// <inheritdoc cref="IUIComponent{TOwner}.ExecuteBehavior{TBehaviorAttribute}(Action{TBehaviorAttribute})"/>
        public void ExecuteBehavior<TBehaviorAttribute>(Action<TBehaviorAttribute> behaviorExecutionAction)
            where TBehaviorAttribute : MulticastAttribute
        {
            behaviorExecutionAction.CheckNotNull(nameof(behaviorExecutionAction));

            var behavior = GetAttributeOrThrow<TBehaviorAttribute>();

            Log.ExecuteSection(
                new ExecuteBehaviorLogSection(this, behavior),
                () => behaviorExecutionAction.Invoke(behavior));
        }

        /// <inheritdoc cref="IUIComponent{TOwner}.ExecuteBehavior{TBehaviorAttribute}(Action{TBehaviorAttribute})"/>
        public TResult ExecuteBehavior<TBehaviorAttribute, TResult>(Func<TBehaviorAttribute, TResult> behaviorExecutionFunction)
            where TBehaviorAttribute : MulticastAttribute
        {
            behaviorExecutionFunction.CheckNotNull(nameof(behaviorExecutionFunction));

            var behavior = GetAttributeOrThrow<TBehaviorAttribute>();

            return Log.ExecuteSection(
                new ExecuteBehaviorLogSection(this, behavior),
                () => behaviorExecutionFunction.Invoke(behavior));
        }

        protected TAttribute GetAttributeOrThrow<TAttribute>() =>
            Metadata.TryGet<TAttribute>(out var attribute)
            ? attribute
            : throw AttributeNotFoundException.Create(typeof(TAttribute), ComponentFullName);
    }
}
