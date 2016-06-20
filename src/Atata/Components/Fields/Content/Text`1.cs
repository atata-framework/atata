namespace Atata
{
    /// <summary>
    /// Represents any element containing text content.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition(ComponentTypeName = "text element")]
    public class Text<TOwner> : Content<string, TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
