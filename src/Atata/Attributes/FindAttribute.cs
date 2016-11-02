using System;

namespace Atata
{
    /// <summary>
    /// Represents the base attribute class for the finding attributes.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public abstract class FindAttribute : Attribute
    {
        private const ScopeSource DefaultScope = ScopeSource.Parent;

        protected FindAttribute()
        {
        }

        /// <summary>
        /// Gets or sets the index of the control. The default value is -1, meaning that the index is not used.
        /// </summary>
        public int Index { get; set; } = -1;

        /// <summary>
        /// Gets or sets the scope source. The default value is Inherit.
        /// </summary>
        public ScopeSource Scope { get; set; }

        /// <summary>
        /// Gets or sets the strategy type for the control search. Strategy type should implement <see cref="IComponentScopeLocateStrategy"/>. The default value is null, meaning that the default strategy of the specific <see cref="FindAttribute"/> should be used.
        /// </summary>
        public Type Strategy { get; set; }

        public abstract IComponentScopeLocateStrategy CreateStrategy(UIComponentMetadata metadata);

        public ScopeSource ResolveScope(UIComponentMetadata metadata)
        {
            return Scope != ScopeSource.Inherit
                ? Scope
                : GetFindSettings(metadata, x => x.Scope != ScopeSource.Inherit)?.Scope ?? DefaultScope;
        }

        public int? ResolveIndex(UIComponentMetadata metadata)
        {
            return Index >= 0
                ? Index
                : GetFindSettings(metadata, x => x.Index >= 0)?.Index;
        }

        private FindSettingsAttribute GetFindSettings(UIComponentMetadata metadata, Func<FindSettingsAttribute, bool> predicate)
        {
            Type thisType = GetType();
            return metadata.GetFirstOrDefaultAttribute<FindSettingsAttribute>(x => x.FindAttributeType == thisType && predicate(x));
        }
    }
}
