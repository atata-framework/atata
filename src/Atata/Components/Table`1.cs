namespace Atata
{
    public class Table<TOwner> : Table<TableRow<TOwner>, TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
