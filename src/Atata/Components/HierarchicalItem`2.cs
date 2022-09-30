namespace Atata
{
    /// <summary>
    /// Represents the hierarchical item control (a control containing structured hierarchy of controls of <typeparamref name="TItem" /> type).
    /// Can have parent control of <typeparamref name="TItem" /> type.
    /// Default search finds the first occurring element.
    /// </summary>
    /// <typeparam name="TItem">The type of the item control.</typeparam>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    /// <seealso cref="HierarchicalControl{TItem, TOwner}" />
    public class HierarchicalItem<TItem, TOwner> : HierarchicalControl<TItem, TOwner>
        where TItem : HierarchicalItem<TItem, TOwner>
        where TOwner : PageObject<TOwner>
    {
        /// <summary>
        /// Gets the parent control of <typeparamref name="TItem"/> type.
        /// </summary>
        [FindLast(OuterXPath = "ancestor::")]
        public new TItem Parent => Controls.Resolve<TItem>(nameof(Parent));

        /// <summary>
        /// Gets the <see cref="ValueProvider{TValue, TOwner}"/> of a value indicating whether the control has parent.
        /// </summary>
        public ValueProvider<bool, TOwner> HasParent =>
            CreateValueProvider("has parent", OnHasParent);

        protected virtual bool OnHasParent() =>
            Parent.Exists(SearchOptions.SafelyAtOnce());
    }
}
