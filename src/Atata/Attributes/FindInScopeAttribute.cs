using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Assembly, Inherited = true)]
    public class FindInScopeAttribute : Attribute
    {
        public FindInScopeAttribute(ScopeSource scope = ScopeSource.Inherit)
        {
            Scope = scope;
        }

        public ScopeSource Scope { get; private set; }
    }
}
