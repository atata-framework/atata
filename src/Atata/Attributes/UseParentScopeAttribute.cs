using System;

namespace Atata
{
    public class UseParentScopeAttribute : FindAttribute
    {
        public new int Index => base.Index;

        protected override Type DefaultStrategy => typeof(UseParentScopeStrategy);
    }
}
