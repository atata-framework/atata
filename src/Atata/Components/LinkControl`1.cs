namespace Atata
{
    [UIComponent("a", IgnoreNameEndings = "Button,Link")]
    public class LinkControl<TOwner> : ClickableControl<TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
