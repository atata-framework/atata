namespace Atata
{
    public class HiddenInput<TOwner> : HiddenInput<string, TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
