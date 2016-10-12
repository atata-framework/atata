namespace Atata
{
    /// <summary>
    /// Represents any element containing number content. Default search is performed by the label.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    public class Number<TOwner> : Content<decimal?, TOwner>
            where TOwner : PageObject<TOwner>
    {
    }
}
