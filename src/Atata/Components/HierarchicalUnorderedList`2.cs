namespace Atata
{
    /// <summary>
    /// Represents the hierarchical unordered list control (&lt;ul&gt;).
    /// Default search finds the first occurring &lt;ul&gt; element.
    /// </summary>
    /// <typeparam name="TItem">The type of the list item control.</typeparam>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    /// <seealso cref="HierarchicalControl{TItem, TOwner}" />
    /// <seealso cref="HierarchicalListItem{TItem, TOwner}" />
    [ControlDefinition("ul", ComponentTypeName = "unordered list")]
    public class HierarchicalUnorderedList<TItem, TOwner> : HierarchicalControl<TItem, TOwner>
        where TItem : HierarchicalItem<TItem, TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
