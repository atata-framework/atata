using System;
using System.Collections.Generic;
using System.Linq;

namespace Atata
{
    /// <summary>
    /// Represents the base attribute class for the finding attributes.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public abstract class FindAttribute : Attribute, IPropertySettings
    {
        private readonly Func<UIComponentMetadata, IEnumerable<IPropertySettings>> findSettingsGetter;

        protected FindAttribute()
        {
            findSettingsGetter = md => md.GetAll<FindSettingsAttribute>(x => x.ForAttribute(GetType()));
        }

        public PropertyBag Properties { get; } = new PropertyBag();

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
        /// Strategy type should implement <see cref="IComponentScopeLocateStrategy"/>.
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
        /// Gets the default strategy type for the control search.
        /// Strategy type should implement <see cref="IComponentScopeLocateStrategy"/>.
        /// </summary>
        protected abstract Type DefaultStrategy { get; }

        [Obsolete("Use CreateStrategy() instead.")] // Obsolete since v1.5.0.
        public IComponentScopeLocateStrategy CreateStrategy(UIComponentMetadata metadata)
        {
            return CreateStrategy();
        }

        public IComponentScopeLocateStrategy CreateStrategy()
        {
            Type strategyType = Strategy ?? DefaultStrategy;
            object[] strategyArguments = GetStrategyArguments().ToArray();

            return (IComponentScopeLocateStrategy)Activator.CreateInstance(strategyType, strategyArguments);
        }

        protected virtual IEnumerable<object> GetStrategyArguments()
        {
            yield break;
        }
    }
}
