namespace Atata
{
    /// <summary>
    /// Represents the link control (&lt;a&gt;). By default is being searched by the content.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("a", ComponentTypeName = "link", IgnoreNameEndings = "Button,Link")]
    [ControlFinding(FindTermBy.Content)]
    public class LinkControl<TOwner> : Control<TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
