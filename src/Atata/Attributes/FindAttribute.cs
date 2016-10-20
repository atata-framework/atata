using System;
using System.Linq;

namespace Atata
{
    /// <summary>
    /// Represents the base attribute class for the finding attributes.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public abstract class FindAttribute : Attribute
    {
        private const ScopeSource DefaultScopeSource = ScopeSource.Parent;

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

        public abstract IComponentScopeLocateStrategy CreateStrategy(UIComponentMetadata metadata);

        public ScopeSource GetScope(UIComponentMetadata metadata)
        {
            return Scope != ScopeSource.Inherit ? Scope : GetScopeFromMetadata(metadata);
        }

        private ScopeSource GetScopeFromMetadata(UIComponentMetadata metadata)
        {
            var scopeAttribute = metadata.DeclaringAttributes.
                Concat(metadata.ParentComponentAttributes).
                OfType<FindInScopeAttribute>().
                FirstOrDefault(x => x.Scope != ScopeSource.Inherit);

            return scopeAttribute != null ? scopeAttribute.Scope : DefaultScopeSource;
        }
    }
}
