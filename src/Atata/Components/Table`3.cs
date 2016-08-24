namespace Atata
{
    [ControlDefinition("table", IgnoreNameEndings = "Table")]
    public class Table<THeader, TRow, TOwner> : Control<TOwner>
        where THeader : TableHeader<TOwner>
        where TRow : TableRow<TOwner>
        where TOwner : PageObject<TOwner>
    {
        public TableRowList<TRow, TOwner> Rows { get; private set; }

        public ControlList<THeader, TOwner> Headers { get; private set; }
    }
}
