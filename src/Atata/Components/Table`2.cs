namespace Atata
{
    public class Table<TRow, TOwner> : Table<TableHeader<TOwner>, TRow, TOwner>
        where TRow : TableRow<TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
