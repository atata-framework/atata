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
        protected FindAttribute()
        {
        }

        public PropertyBag Properties { get; } = new PropertyBag();

        /// <summary>
        /// Gets or sets the index of the control. The default value is -1, meaning that the index is not used.
        /// </summary>
        public int Index
        {
            get
            {
                return Properties.Get(
                    nameof(Index),
                    -1,
                    md => md.GetDeclaringAndGlobalAttributes<FindSettingsAttribute>(x => x.FindAttributeType == GetType()));
            }

            set
            {
                Properties[nameof(Index)] = value;
            }
        }

        /// <summary>
        /// Gets or sets the visibility. The default value is Visible.
        /// </summary>
        public Visibility Visibility
        {
            get
            {
                return Properties.Get(
                    nameof(Visibility),
                    Visibility.Visible,
                    md => md.GetDeclaringAndGlobalAttributes<FindSettingsAttribute>(x => x.FindAttributeType == GetType()),
                    md => md.GetComponentAttributes<ControlDefinitionAttribute>());
            }

            set
            {
                Properties[nameof(Visibility)] = value;
            }
        }

        /// <summary>
        /// Gets or sets the scope source. The default value is Parent.
        /// </summary>
        public ScopeSource ScopeSource
        {
            get
            {
                return Properties.Get(
                    nameof(ScopeSource),
                    ScopeSource.Parent,
                    md => md.GetDeclaringAndGlobalAttributes<FindSettingsAttribute>(x => x.FindAttributeType == GetType()));
            }

            set
            {
                Properties[nameof(ScopeSource)] = value;
            }
        }

        /// <summary>
        /// Gets or sets the strategy type for the control search. Strategy type should implement <see cref="IComponentScopeLocateStrategy"/>. The default value is null, meaning that the default strategy of the specific <see cref="FindAttribute"/> should be used.
        /// </summary>
        public Type Strategy { get; set; }

        /// <summary>
        /// Gets the default strategy type for the control search. Strategy type should implement <see cref="IComponentScopeLocateStrategy"/>.
        /// </summary>
        protected abstract Type DefaultStrategy { get; }

        public IComponentScopeLocateStrategy CreateStrategy(UIComponentMetadata metadata)
        {
            Type strategyType = ResolveStrategyType(metadata);
            object[] strategyArguments = GetStrategyArguments().ToArray();

            return (IComponentScopeLocateStrategy)Activator.CreateInstance(strategyType, strategyArguments);
        }

        protected virtual IEnumerable<object> GetStrategyArguments()
        {
            yield break;
        }

        public Type ResolveStrategyType(UIComponentMetadata metadata)
        {
            return Strategy ?? GetFindSettings(metadata, x => x.Strategy != null)?.Strategy ?? DefaultStrategy;
        }

        private FindSettingsAttribute GetFindSettings(UIComponentMetadata metadata, Func<FindSettingsAttribute, bool> predicate)
        {
            Type thisType = GetType();
            return metadata.GetFirstOrDefaultDeclaringAttribute<FindSettingsAttribute>(x => x.FindAttributeType == thisType && predicate(x))
                ?? metadata.GetFirstOrDefaultGlobalAttribute<FindSettingsAttribute>(x => x.FindAttributeType == thisType && predicate(x));
        }
    }
}
