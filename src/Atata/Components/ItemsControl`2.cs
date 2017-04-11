namespace Atata
{
    /// <summary>
    /// Represents the items control (a set of any control of <typeparamref name="TItem"/> type).
    /// </summary>
    /// <typeparam name="TItem">The type of the item control.</typeparam>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition(IgnoreNameEndings = "ItemsControl,Items,Control")]
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
