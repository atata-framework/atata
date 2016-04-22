namespace Atata
{
    public class Select<TOwner> : Select<string, TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
