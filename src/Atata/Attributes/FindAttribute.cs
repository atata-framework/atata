using System;
using System.Linq;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public abstract class FindAttribute : Attribute
    {
        private const ScopeSource DefaultScopeSource = ScopeSource.Parent;

        protected FindAttribute()
        {
        }

        public int Index { get; set; }
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
