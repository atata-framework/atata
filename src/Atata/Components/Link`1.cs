namespace Atata
{
    /// <summary>
    /// Represents the link control (&lt;a&gt;). Default search is performed by the content.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("a", ComponentTypeName = "link", IgnoreNameEndings = "Button,Link")]
    [ControlFinding(FindTermBy.Content)]
    public class Link<TOwner> : Control<TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
