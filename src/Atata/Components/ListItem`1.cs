namespace Atata
{
    /// <summary>
    /// Represents the list item control (<c>&lt;li&gt;</c>).
    /// Default search finds the first occurring <c>&lt;li&gt;</c> element.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    /// <seealso cref="OrderedList{TItem, TOwner}" />
    /// <seealso cref="UnorderedList{TItem, TOwner}" />
    [ListItemDefinition]
    public class ListItem<TOwner> : Control<TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
