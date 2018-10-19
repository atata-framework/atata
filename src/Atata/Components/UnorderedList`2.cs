namespace Atata
{
    /// <summary>
    /// Represents the unordered list control (&lt;ul&gt;).
    /// Default search finds the first occurring &lt;ul&gt; element.
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
}
