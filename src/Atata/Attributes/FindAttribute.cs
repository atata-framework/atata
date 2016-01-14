using System;

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

        public abstract IElementFindStrategy CreateStrategy(UIPropertyMetadata metadata);

        public ScopeSource GetScope(UIPropertyMetadata metadata)
        {
            return Scope != ScopeSource.Inherit ? Scope : GetScopeFromMetadata(metadata);
        }

        private ScopeSource GetScopeFromMetadata(UIPropertyMetadata metadata)
        {
            var scopeAttribute = metadata.GetFirstOrDefaultAttribute<FindInScope>(x => x.Scope != ScopeSource.Inherit);
            return scopeAttribute != null ? scopeAttribute.Scope : DefaultScopeSource;
        }
    }
}
