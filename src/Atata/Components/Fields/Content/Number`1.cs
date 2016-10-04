namespace Atata
{
    /// <summary>
    /// Represents any element containing number content. By default is being searched by the label (if is declared in the class inherited from `TableRow`, then by column header).
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    public class Number<TOwner> : Content<decimal?, TOwner>
            where TOwner : PageObject<TOwner>
    {
    }
}
