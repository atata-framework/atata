namespace Atata
{
    [UIComponent("a", IgnoreNameEndings = "Button,Link")]
    public class Link<TOwner> : Clickable<TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
