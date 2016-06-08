namespace Atata
{
    /// <summary>
    /// Represents any element containing text content.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    public class Text<TOwner> : Content<string, TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
