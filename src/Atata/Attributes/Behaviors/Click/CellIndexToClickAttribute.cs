using System;

namespace Atata
{
    [Obsolete("Use " + nameof(ClickOnCellByIndexAttribute) + " instead.")] // Obsolete since v1.9.0.
    public class CellIndexToClickAttribute : ClickOnCellByIndexAttribute
    {
        public CellIndexToClickAttribute(int index)
            : base(index)
        {
        }
    }
}
