namespace Atata
{
    public class TableRow<TOwner> : TableRow<TOwner, TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
