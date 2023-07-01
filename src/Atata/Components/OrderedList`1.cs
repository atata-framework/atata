namespace Atata;

/// <summary>
/// Represents the ordered list control (<c>&lt;ol&gt;</c>).
/// Default search finds the first occurring <c>&lt;ol&gt;</c> element.
/// </summary>
/// <typeparam name="TOwner">The type of the owner page object.</typeparam>
/// <seealso cref="ItemsControl{TItem, TOwner}" />
/// <seealso cref="TextListItem{TOwner}" />
public class OrderedList<TOwner> : OrderedList<TextListItem<TOwner>, TOwner>
    where TOwner : PageObject<TOwner>
{
}
