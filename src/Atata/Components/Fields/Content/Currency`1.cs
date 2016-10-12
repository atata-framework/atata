namespace Atata
{
    /// <summary>
    /// Represents any element containing currency content. Default search is performed by the label (if is declared in the class inherited from <see cref="TableRow{TOwner}"/>,then by column header).
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [Format("C2")]
    public class Currency<TOwner> : Content<decimal?, TOwner>
            where TOwner : PageObject<TOwner>
    {
    }
}
