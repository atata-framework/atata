using System;
using System.Collections.Generic;
using System.Linq;

namespace Atata
{
    /// <summary>
    /// Represents the base attribute class for the finding attributes.
    /// </summary>
    public abstract class FindAttribute : MulticastAttribute
    {
        private readonly Func<UIComponentMetadata, IEnumerable<IPropertySettings>> _findSettingsGetter;

        protected FindAttribute()
        {
            _findSettingsGetter = md => md.GetAll<FindSettingsAttribute>(x => x.ForAttribute(GetType()));
        }

        /// <summary>
        /// Gets or sets the index of the control.
        /// The default value is <c>-1</c>, meaning that the index is not used.
        /// </summary>
        public int Index
        {
            get
            {
                return Properties.Get(
                    nameof(Index),
                    -1,
                    _findSettingsGetter);
            }

            set
            {
                Properties[nameof(Index)] = value;
            }
        }

        /// <summary>
        /// Gets or sets the visibility.
        /// The default value is <see cref="Visibility.Visible"/>.
        /// </summary>
        public Visibility Visibility
        {
            get
            {
                return Properties.Get(
                    nameof(Visibility),
                    Visibility.Visible,
                    _findSettingsGetter,
                    md => new[] { md.ComponentDefinitionAttribute });
            }

            set
            {
                Properties[nameof(Visibility)] = value;
            }
        }

        /// <summary>
        /// Gets or sets the scope source.
        /// The default value is <see cref="ScopeSource.Parent"/>.
        /// </summary>
        public ScopeSource ScopeSource
        {
            get
            {
                return Properties.Get(
                    nameof(ScopeSource),
                    ScopeSource.Parent,
                    _findSettingsGetter);
            }

            set
            {
                Properties[nameof(ScopeSource)] = value;
            }
        }

        /// <summary>
        /// Gets or sets the outer XPath.
        /// The default value is <see langword="null"/>, meaning that the control is searchable as descendant (using <c>".//"</c> XPath) in scope source.
        /// </summary>
        public string OuterXPath
        {
            get
            {
                return Properties.Get<string>(
                    nameof(OuterXPath),
                    _findSettingsGetter);
            }

            set
            {
                Properties[nameof(OuterXPath)] = value;
            }
        }

        /// <summary>
        /// Gets or sets the strategy type for the control search.
        /// Strategy type should implement <see cref="IComponentScopeFindStrategy"/>.
        /// The default value is <see langword="null"/>, meaning that the default strategy of the specific <see cref="FindAttribute"/> should be used.
        /// </summary>
        public Type Strategy
        {
            get
            {
                return Properties.Get(
                    nameof(Strategy),
                    DefaultStrategy,
                    _findSettingsGetter);
            }

            set
            {
                Properties[nameof(Strategy)] = value;
            }
        }

        /// <summary>
        /// Gets or sets the element find timeout in seconds.
        /// The default value is taken from <see cref="AtataContext.ElementFindTimeout"/> property of <see cref="AtataContext.Current"/>.
        /// </summary>
        public double Timeout
        {
            get => Properties.Get<double?>(nameof(Timeout), _findSettingsGetter)
                ?? (AtataContext.Current?.ElementFindTimeout ?? RetrySettings.Timeout).TotalSeconds;
            set => Properties[nameof(Timeout)] = value;
        }

        /// <summary>
        /// Gets or sets the element find retry interval in seconds.
        /// The default value is taken from <see cref="AtataContext.ElementFindRetryInterval"/> property of <see cref="AtataContext.Current"/>.
        /// </summary>
        public double RetryInterval
        {
            get => Properties.Get<double?>(nameof(RetryInterval), _findSettingsGetter)
                ?? (AtataContext.Current?.ElementFindRetryInterval ?? RetrySettings.Interval).TotalSeconds;
            set => Properties[nameof(RetryInterval)] = value;
        }

        /// <summary>
        /// Gets or sets the way this <see cref="FindAttribute"/> should be used.
        /// The default value is <see cref="FindAs.Scope"/>.
        /// Each control can have 1 <see cref="FindAttribute"/> with <see cref="FindAs.Scope"/> value
        /// and many other <see cref="FindAttribute"/>s with another <see cref="FindAs"/> values, which are used as layers.
        /// When several layer attributes are used,
        /// then <see cref="Layer"/> property can be used to specify an order of each attribute.
        /// </summary>
        public FindAs As
        {
            get
            {
                return Properties.Get(
                    nameof(As),
                    FindAs.Scope);
            }

            set
            {
                Properties[nameof(As)] = value;
            }
        }

        /// <summary>
        /// Gets or sets the layer order of find attribute.
        /// It is useful to specify the order of the layer when several layers are used.
        /// This property is used only paired with <see cref="As"/> property set to any value except <see cref="FindAs.Scope"/>.
        /// The default value is <c>0</c>.
        /// </summary>
        public int Layer
        {
            get
            {
                return Properties.Get(
                    nameof(Layer),
                    0);
            }

            set
            {
                Properties[nameof(Layer)] = value;
            }
        }

        /// <summary>
        /// Gets the default strategy type for the control search.
        /// Strategy type should implement <see cref="IComponentScopeFindStrategy"/>.
        /// </summary>
        protected abstract Type DefaultStrategy { get; }

        /// <summary>
        /// Creates the strategy.
        /// </summary>
        /// <returns>The strategy created.</returns>
        public IComponentScopeFindStrategy CreateStrategy()
        {
            Type strategyType = Strategy ?? DefaultStrategy;
            object[] strategyArguments = GetStrategyArguments().ToArray();

            return (IComponentScopeFindStrategy)Activator.CreateInstance(strategyType, strategyArguments);
        }

        protected virtual IEnumerable<object> GetStrategyArguments()
        {
            yield break;
        }

        public virtual string BuildComponentName() =>
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
    }
}
