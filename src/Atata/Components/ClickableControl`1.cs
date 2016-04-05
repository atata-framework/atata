namespace Atata
{
    public class ClickableControl<TOwner> : ClickableControl<TOwner, TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
