namespace Atata
{
    /// <summary>
    /// Represents the table control (&lt;table&gt;). By default is being searched the first occurrence.
    /// </summary>
    /// <typeparam name="THeader">The type of the table header control.</typeparam>
    /// <typeparam name="TRow">The type of the table row control.</typeparam>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("table", IgnoreNameEndings = "Table")]
    public class Table<THeader, TRow, TOwner> : Control<TOwner>
        where THeader : TableHeader<TOwner>
        where TRow : TableRow<TOwner>
        where TOwner : PageObject<TOwner>
    {
        /// <summary>
        /// Gets the rows list.
        /// </summary>
        public TableRowList<TRow, TOwner> Rows { get; private set; }

        /// <summary>
        /// Gets the headers list.
        /// </summary>
        public ControlList<THeader, TOwner> Headers { get; private set; }
    }
}
