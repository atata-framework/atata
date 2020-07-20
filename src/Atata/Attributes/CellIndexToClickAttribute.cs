using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    public class CellIndexToClickAttribute : Attribute
    {
        public CellIndexToClickAttribute(int index)
        {
            Index = index;
        }

        public int Index { get; }
    }
}
