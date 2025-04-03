#nullable enable

namespace Atata;

/// <summary>
/// Represents the hierarchical ordered list control (<c>&lt;ol&gt;</c>).
/// Default search finds the first occurring <c>&lt;ol&gt;</c> element.
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
