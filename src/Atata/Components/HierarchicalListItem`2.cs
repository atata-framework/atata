#nullable enable

namespace Atata;

/// <summary>
/// Represents the hierarchical list item control (<c>&lt;li&gt;</c>).
/// Default search finds the first occurring <c>&lt;li&gt;</c> element.
/// It is recommended to use with <see cref="HierarchicalUnorderedList{TItem, TOwner}"/> and <see cref="HierarchicalOrderedList{TItem, TOwner}"/>.
/// </summary>
/// <typeparam name="TItem">The type of the list item control.</typeparam>
/// <typeparam name="TOwner">The type of the owner page object.</typeparam>
/// <seealso cref="HierarchicalOrderedList{TItem, TOwner}" />
/// <seealso cref="HierarchicalUnorderedList{TItem, TOwner}" />
[ControlDefinition("li", ComponentTypeName = "list item")]
[FindSettings(OuterXPath = "(./ul | ./ol)/", TargetName = nameof(Children))]
public class HierarchicalListItem<TItem, TOwner> : HierarchicalItem<TItem, TOwner>
    where TItem : HierarchicalListItem<TItem, TOwner>
    where TOwner : PageObject<TOwner>
{
}
