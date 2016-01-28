namespace Atata
{
    [UIComponent("*", IgnoreNameEndings = "Button,Link")]
    public class Clickable<TOwner> : ClickableBase<TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
