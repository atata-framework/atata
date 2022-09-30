using System;
using System.Collections.Generic;
using System.Linq;

namespace Atata
{
    /// <summary>
    /// Represents the base attribute class for the finding attributes.
    /// </summary>
    public abstract class FindAttribute : MulticastAttribute, IHasOptionalProperties
    {
        protected FindAttribute()
        {
        }

        PropertyBag IHasOptionalProperties.OptionalProperties => OptionalProperties;

        protected internal PropertyBag OptionalProperties { get; } = new PropertyBag();

        /// <summary>
        /// Gets or sets the index of the control.
        /// The default value is <c>-1</c>, meaning that the index is not used.
        /// </summary>
        public int Index
        {
            get => ResolveIndex();
            set => OptionalProperties[nameof(Index)] = value;
        }

        /// <summary>
        /// Gets or sets the visibility.
        /// The default value is <see cref="Visibility.Any"/>.
        /// </summary>
        public Visibility Visibility
        {
            get => ResolveVisibility();
            set => OptionalProperties[nameof(Visibility)] = value;
        }

        /// <summary>
        /// Gets or sets the scope source.
        /// The default value is <see cref="ScopeSource.Parent"/>.
        /// </summary>
        public ScopeSource ScopeSource
        {
            get => ResolveScopeSource();
            set => OptionalProperties[nameof(ScopeSource)] = value;
        }

        /// <summary>
        /// Gets or sets the outer XPath.
        /// The default value is <see langword="null"/>, meaning that the control is searchable as descendant (using <c>".//"</c> XPath) in scope source.
        /// </summary>
        public string OuterXPath
        {
            get => ResolveOuterXPath();
            set => OptionalProperties[nameof(OuterXPath)] = value;
        }

        /// <summary>
        /// Gets or sets the strategy type for the control search.
        /// Strategy type should implement <see cref="IComponentScopeFindStrategy"/>.
        /// The default value is <see langword="null"/>, meaning that the default strategy of the specific <see cref="FindAttribute"/> should be used.
        /// </summary>
        public Type Strategy
        {
            get => ResolveStrategy();
            set => OptionalProperties[nameof(Strategy)] = value;
        }

        /// <summary>
        /// Gets or sets the element find timeout in seconds.
        /// The default value is taken from <see cref="AtataContext.ElementFindTimeout"/> property of <see cref="AtataContext.Current"/>.
        /// </summary>
        public double Timeout
        {
            get => ResolveTimeout();
            set => OptionalProperties[nameof(Timeout)] = value;
        }

        /// <summary>
        /// Gets or sets the element find retry interval in seconds.
        /// The default value is taken from <see cref="AtataContext.ElementFindRetryInterval"/> property of <see cref="AtataContext.Current"/>.
        /// </summary>
        public double RetryInterval
        {
            get => ResolveRetryInterval();
            set => OptionalProperties[nameof(RetryInterval)] = value;
        }

        /// <summary>
        /// Gets or sets the way this <see cref="FindAttribute"/> should be used.
        /// The default value is <see cref="FindAs.Scope"/>.
        /// Each control can have 1 <see cref="FindAttribute"/> with <see cref="FindAs.Scope"/> value
        /// and many other <see cref="FindAttribute"/>s with another <see cref="FindAs"/> values, which are used as layers.
        /// When several layer attributes are used,
        /// then <see cref="Layer"/> property can be used to specify an order of each attribute.
        /// </summary>
        public FindAs As { get; set; }

        /// <summary>
        /// Gets or sets the layer order of find attribute.
        /// It is useful to specify the order of the layer when several layers are used.
        /// This property is used only paired with <see cref="As"/> property set to any value except <see cref="FindAs.Scope"/>.
        /// The default value is <c>0</c>.
        /// </summary>
        public int Layer { get; set; }

        /// <summary>
        /// Gets the default strategy type for the control search.
        /// Strategy type should implement <see cref="IComponentScopeFindStrategy"/>.
        /// </summary>
        protected abstract Type DefaultStrategy { get; }

        /// <summary>
        /// Creates the strategy.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        /// <returns>
        /// The strategy created.
        /// </returns>
        public IComponentScopeFindStrategy CreateStrategy(UIComponentMetadata metadata)
        {
            Type strategyType = ResolveStrategy(metadata);
            object[] strategyArguments = GetStrategyArguments().ToArray();

            return (IComponentScopeFindStrategy)Activator.CreateInstance(strategyType, strategyArguments);
        }

        protected virtual IEnumerable<object> GetStrategyArguments()
        {
            yield break;
        }

        public virtual string BuildComponentName(UIComponentMetadata metadata) =>
            GetTypeNameForComponentName();

        protected string GetTypeNameForComponentName()
        {
            string typeName = GetType().Name;

            return typeName.EndsWith(nameof(Attribute), StringComparison.Ordinal)
                ? typeName.Substring(0, typeName.Length - nameof(Attribute).Length)
                : typeName;
        }

        protected string BuildComponentNameWithArgument(object argument) =>
            $"{GetTypeNameForComponentName()}:{argument}";

        internal int ResolveIndex(UIComponentMetadata metadata = null) =>
            OptionalProperties.Resolve(
                nameof(Index),
                -1,
                metadata != null ? GetFindSettingsPropertyAttributes(metadata) : null);

        internal Visibility ResolveVisibility(UIComponentMetadata metadata = null) =>
            OptionalProperties.Resolve<Visibility?>(
                nameof(Visibility),
                metadata != null ? GetFindSettingsPropertyAttributes(metadata).Concat(new[] { (IHasOptionalProperties)metadata.ComponentDefinitionAttribute }) : null)
            ?? AtataContext.Current?.DefaultControlVisibility
            ?? SearchOptions.DefaultVisibility;

        internal ScopeSource ResolveScopeSource(UIComponentMetadata metadata = null) =>
            OptionalProperties.Resolve<ScopeSource>(
                nameof(ScopeSource),
                metadata != null ? GetFindSettingsPropertyAttributes(metadata) : null);

        internal string ResolveOuterXPath(UIComponentMetadata metadata = null) =>
            OptionalProperties.Resolve<string>(
                nameof(OuterXPath),
                metadata != null ? GetFindSettingsPropertyAttributes(metadata) : null);

        internal Type ResolveStrategy(UIComponentMetadata metadata = null) =>
            OptionalProperties.Resolve(
                nameof(Strategy),
                DefaultStrategy,
                metadata != null ? GetFindSettingsPropertyAttributes(metadata) : null);

        internal double ResolveTimeout(UIComponentMetadata metadata = null) =>
            OptionalProperties.Resolve<double?>(
                nameof(Timeout),
                metadata != null ? GetFindSettingsPropertyAttributes(metadata) : null)
                ?? (AtataContext.Current?.ElementFindTimeout ?? RetrySettings.Timeout).TotalSeconds;

        internal double ResolveRetryInterval(UIComponentMetadata metadata = null) =>
            OptionalProperties.Resolve<double?>(
                nameof(RetryInterval),
                metadata != null ? GetFindSettingsPropertyAttributes(metadata) : null)
                ?? (AtataContext.Current?.ElementFindRetryInterval ?? RetrySettings.Interval).TotalSeconds;

        private IEnumerable<IHasOptionalProperties> GetFindSettingsPropertyAttributes(UIComponentMetadata metadata) =>
            metadata.GetAll<FindSettingsAttribute>(x => x.ForAttribute(GetType()));
    }
}
