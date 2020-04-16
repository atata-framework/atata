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
        private readonly Func<UIComponentMetadata, IEnumerable<IPropertySettings>> findSettingsGetter;

        protected FindAttribute()
        {
            findSettingsGetter = md => md.GetAll<FindSettingsAttribute>(x => x.ForAttribute(GetType()));
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
                    findSettingsGetter);
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
                    findSettingsGetter,
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
                    findSettingsGetter);
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
                    findSettingsGetter);
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
                    findSettingsGetter);
            }

            set
            {
                Properties[nameof(Strategy)] = value;
            }
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

        [Obsolete("Use CreateStrategy() instead.")] // Obsolete since v1.5.0.
        public IComponentScopeLocateStrategy CreateStrategy(UIComponentMetadata metadata)
        {
            return (IComponentScopeLocateStrategy)CreateStrategy();
        }

        /// <summary>
        /// Creates the strategy.
        /// The type of strategy should be either <see cref="IComponentScopeFindStrategy"/> or <see cref="IComponentScopeLocateStrategy"/>.
        /// </summary>
        /// <returns>The strategy created.</returns>
        public object CreateStrategy()
        {
            Type strategyType = Strategy ?? DefaultStrategy;
            object[] strategyArguments = GetStrategyArguments().ToArray();

            return Activator.CreateInstance(strategyType, strategyArguments);
        }

        protected virtual IEnumerable<object> GetStrategyArguments()
        {
            yield break;
        }
    }
}
