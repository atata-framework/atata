#nullable enable

namespace Atata;

/// <summary>
/// Represents the list item control (<c>&lt;li&gt;</c>) of text kind.
/// Default search finds the first occurring <c>&lt;li&gt;</c> element.
/// </summary>
/// <typeparam name="TOwner">The type of the owner page object.</typeparam>
/// <seealso cref="OrderedList{TOwner}" />
/// <seealso cref="UnorderedList{TOwner}" />
[ListItemDefinition]
public class TextListItem<TOwner> : Text<TOwner>
    where TOwner : PageObject<TOwner>
{
}
