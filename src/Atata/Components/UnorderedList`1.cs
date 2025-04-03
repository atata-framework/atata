#nullable enable

namespace Atata;

/// <summary>
/// Represents the unordered list control (<c>&lt;ul&gt;</c>).
/// Default search finds the first occurring <c>&lt;ul&gt;</c> element.
/// </summary>
/// <typeparam name="TOwner">The type of the owner page object.</typeparam>
/// <seealso cref="ItemsControl{TItem, TOwner}" />
/// <seealso cref="TextListItem{TOwner}" />
public class UnorderedList<TOwner> : UnorderedList<TextListItem<TOwner>, TOwner>
    where TOwner : PageObject<TOwner>
{
}
