namespace Atata
{
    /// <summary>
    /// Represents the table control (&lt;table&gt;). By default is being searched the first occurrence.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    public class Table<TOwner> : Table<TableHeader<TOwner>, TableRow<TOwner>, TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
