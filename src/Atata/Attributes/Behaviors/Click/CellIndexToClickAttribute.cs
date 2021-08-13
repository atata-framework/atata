using System;

namespace Atata
{
    [Obsolete("Use " + nameof(ClicksOnCellByIndexAttribute) + " instead.")] // Obsolete since v1.9.0.
    public class CellIndexToClickAttribute : ClickOnCellByIndexAttribute
    {
        public CellIndexToClickAttribute(int index)
            : base(index)
        {
        }
    }
}
