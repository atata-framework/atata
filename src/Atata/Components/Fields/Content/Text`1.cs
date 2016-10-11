namespace Atata
{
    /// <summary>
    /// Represents any element containing text content. Default search is performed by the label (if is declared in the class inherited from `TableRow`, then by column header).
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    public class Text<TOwner> : Content<string, TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
