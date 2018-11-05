namespace Atata
{
    /// <summary>
    /// Represents any element containing currency content.
    /// Default search finds the first occurring element.
    /// The default format is <c>C2</c> (e.g. <c>$123.45</c>).
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [Format("C2")]
    public class Currency<TOwner> : Content<decimal?, TOwner>
            where TOwner : PageObject<TOwner>
    {
    }
}
