namespace Atata
{
    /// <summary>
    /// Represents the ordered list control (&lt;ol&gt;). Default search finds the first occurring &lt;ol&gt; element.
    /// </summary>
    /// <typeparam name="TItem">The type of the list item control.</typeparam>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    /// <seealso cref="ItemsControl{TItem, TOwner}" />
    /// <seealso cref="ListItem{TOwner}" />
    [ControlDefinition("ol", ComponentTypeName = "ordered list")]
    [FindSettings(OuterXPath = "./", TargetName = nameof(Items))]
    public class OrderedList<TItem, TOwner> : ItemsControl<TItem, TOwner>
        where TItem : Control<TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
