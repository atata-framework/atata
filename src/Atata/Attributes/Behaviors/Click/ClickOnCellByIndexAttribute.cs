using System;

namespace Atata
{
    /// <summary>
    /// Represents the behavior for control clicking by actually clicking the nth <c>&lt;td&gt;</c> cell.
    /// There is a sense to apply this behavior to <see cref="TableRow{TOwner}"/> classes.
    /// </summary>
    [Obsolete("Use " + nameof(ClicksOnCellByIndexAttribute) + " instead.")] // Obsolete since v1.12.0.
    public class ClickOnCellByIndexAttribute : ClicksOnCellByIndexAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClickOnCellByIndexAttribute"/> class.
        /// </summary>
        /// <param name="index">The index of a cell.</param>
        public ClickOnCellByIndexAttribute(int index)
            : base(index)
        {
        }
    }
}
