namespace Atata
{
    /// <summary>
    /// Represents the link control (<c>&lt;a&gt;</c>).
    /// Default search is performed by the content.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("a", ComponentTypeName = "link", IgnoreNameEndings = "Button,Link")]
    [FindByContent]
    public class Link<TOwner> : Control<TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
