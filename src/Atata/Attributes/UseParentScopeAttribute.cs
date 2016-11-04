using System;

namespace Atata
{
    public class UseParentScopeAttribute : FindAttribute
    {
        public new int Index
        {
            get { return base.Index; }
        }

        protected override Type DefaultStrategy
        {
            get { return typeof(UseParentScopeStrategy); }
        }
    }
}
