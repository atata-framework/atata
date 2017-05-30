namespace Atata
{
    /// <summary>
    /// Represents the items control (a control containing a set of any control of <typeparamref name="TItem"/> type). Default search finds the first occurring element.
    /// </summary>
    /// <typeparam name="TItem">The type of the item control.</typeparam>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition(ComponentTypeName = "items control", IgnoreNameEndings = "ItemsControl,Control")]
    public class ItemsControl<TItem, TOwner> : Control<TOwner>
        where TItem : Control<TOwner>
        where TOwner : PageObject<TOwner>
    {
        /// <summary>
        /// Gets the items' <see cref="ControlList{TItem, TOwner}"/> instance.
        /// </summary>
        public ControlList<TItem, TOwner> Items { get; private set; }
    }
}
