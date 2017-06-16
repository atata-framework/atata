namespace Atata
{
    /// <summary>
    /// Represents the hierarchical ordered list control (&lt;ol&gt;). Default search finds the first occurring &lt;ol&gt; element.
    /// </summary>
    /// <typeparam name="TItem">The type of the list item control.</typeparam>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    /// <seealso cref="HierarchicalControl{TItem, TOwner}" />
    /// <seealso cref="HierarchicalListItem{TItem, TOwner}" />
    [ControlDefinition("ol", ComponentTypeName = "ordered list")]
    public class HierarchicalOrderedList<TItem, TOwner> : HierarchicalControl<TItem, TOwner>
        where TItem : HierarchicalItem<TItem, TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
