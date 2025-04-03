#nullable enable

namespace Atata;

/// <summary>
/// Represents the hierarchical list item control (<c>&lt;li&gt;</c>).
/// Default search finds the first occurring <c>&lt;li&gt;</c> element.
/// It is recommended to use with <see cref="HierarchicalUnorderedList{TItem, TOwner}"/> and <see cref="HierarchicalOrderedList{TItem, TOwner}"/>.
/// </summary>
/// <typeparam name="TOwner">The type of the owner page object.</typeparam>
/// <seealso cref="HierarchicalOrderedList{TItem, TOwner}" />
/// <seealso cref="HierarchicalUnorderedList{TItem, TOwner}" />
public class HierarchicalListItem<TOwner> : HierarchicalListItem<HierarchicalListItem<TOwner>, TOwner>
    where TOwner : PageObject<TOwner>
{
}
