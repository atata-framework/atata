namespace Atata
{
    /// <summary>
    /// Represents the behavior for control clicking by actually clicking the nth <c>&lt;td&gt;</c> cell.
    /// There is a sense to apply this behavior to <see cref="TableRow{TOwner}"/> classes.
    /// </summary>
    public class ClicksOnCellByIndexAttribute : ClickBehaviorAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClicksOnCellByIndexAttribute"/> class.
        /// </summary>
        /// <param name="index">The index of a cell.</param>
        public ClicksOnCellByIndexAttribute(int index) =>
            Index = index;

        /// <summary>
        /// Gets the index of a cell.
        /// </summary>
        public int Index { get; }

        /// <inheritdoc/>
        public override void Execute<TOwner>(IUIComponent<TOwner> component)
        {
            var cellControl = component.Find<Control<TOwner>>(
                new ControlDefinitionAttribute("td") { ComponentTypeName = "cell" },
                new FindByIndexAttribute(Index));

            cellControl.Click();
        }
    }
}
