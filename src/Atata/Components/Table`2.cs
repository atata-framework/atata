namespace Atata
{
    [ControlDefinition("table", IgnoreNameEndings = "Table")]
    public class Table<TRow, TOwner> : Control<TOwner>
        where TRow : TableRow<TOwner>, new()
        where TOwner : PageObject<TOwner>
    {
        public TableRowList<TRow, TOwner> Rows { get; private set; }

        public ControlList<TableHeader<TOwner>, TOwner> Headers { get; private set; }
    }
}
