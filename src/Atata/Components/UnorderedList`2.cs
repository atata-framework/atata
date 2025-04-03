#nullable enable

namespace Atata;

/// <summary>
/// Represents the unordered list control (<c>&lt;ul&gt;</c>).
/// Default search finds the first occurring <c>&lt;ul&gt;</c> element.
/// </summary>
/// <typeparam name="TItem">The type of the list item control.</typeparam>
/// <typeparam name="TOwner">The type of the owner page object.</typeparam>
/// <seealso cref="ItemsControl{TItem, TOwner}" />
/// <seealso cref="ListItem{TOwner}" />
[ControlDefinition("ul", ComponentTypeName = "unordered list")]
[FindSettings(OuterXPath = "./", TargetName = nameof(Items))]
public class UnorderedList<TItem, TOwner> : ItemsControl<TItem, TOwner>
    where TItem : Control<TOwner>
    where TOwner : PageObject<TOwner>
{
}
