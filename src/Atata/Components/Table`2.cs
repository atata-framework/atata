namespace Atata
{
    /// <summary>
    /// Represents the table control (&lt;table&gt;). Default search finds the first occuring table.
    /// </summary>
    /// <typeparam name="TRow">The type of the table row control.</typeparam>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    public class Table<TRow, TOwner> : Table<TableHeader<TOwner>, TRow, TOwner>
        where TRow : TableRow<TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
