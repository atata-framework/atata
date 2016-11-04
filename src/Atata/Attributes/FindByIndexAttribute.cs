using System;

namespace Atata
{
    /// <summary>
    /// Specifies that a control should use the nth occurring element matching the control's definition.
    /// </summary>
    public class FindByIndexAttribute : FindAttribute
    {
        public FindByIndexAttribute(int index)
        {
            Index = index;
        }

        protected override Type DefaultStrategy
        {
            get { return typeof(FindByIndexStrategy); }
        }
    }
}
