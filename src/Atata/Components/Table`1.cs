namespace Atata
{
    public class Table<TOwner> : Table<TableHeader<TOwner>, TableRow<TOwner>, TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
