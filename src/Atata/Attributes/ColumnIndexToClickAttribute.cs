using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    public class ColumnIndexToClickAttribute : Attribute
    {
        public ColumnIndexToClickAttribute(int index)
        {
            Index = index;
        }

        public int Index { get; private set; }
    }
}
