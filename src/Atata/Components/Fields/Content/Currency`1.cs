namespace Atata
{
    /// <summary>
    /// Represents any element containing currency content. Default search finds the first occurring element. The default format is "C2" (e.g. $123.45).
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [Format("C2")]
    public class Currency<TOwner> : Content<decimal?, TOwner>
            where TOwner : PageObject<TOwner>
    {
    }
}
