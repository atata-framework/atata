namespace Atata
{
    /// <summary>
    /// Represents the table control (&lt;table&gt;). Default search finds the first occurring &lt;table&gt; element.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    public class Table<TOwner> : Table<TableHeader<TOwner>, TableRow<TOwner>, TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
