using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Assembly, Inherited = true)]
    public class FindInScope : Attribute
    {
        public FindInScope(ScopeSource scope = ScopeSource.Inherit)
        {
            Scope = scope;
        }

        public ScopeSource Scope { get; set; }
    }
}
