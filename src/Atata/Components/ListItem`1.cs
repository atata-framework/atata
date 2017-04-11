namespace Atata
{
    /// <summary>
    /// Represents the list item control (&lt;li&gt;). Default search finds the first occurring &lt;li&gt; element.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    /// <seealso cref="Atata.OrderedList{TItem, TOwner}" />
    /// <seealso cref="Atata.UnorderedList{TItem, TOwner}" />
    [ControlDefinition("li", ComponentTypeName = "list item")]
    public class ListItem<TOwner> : Control<TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
