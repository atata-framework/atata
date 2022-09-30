namespace Atata
{
    /// <summary>
    /// Represents any element containing number content.
    /// Default search finds the first occurring element.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    public class Number<TOwner> : Content<decimal?, TOwner>
            where TOwner : PageObject<TOwner>
    {
    }
}
