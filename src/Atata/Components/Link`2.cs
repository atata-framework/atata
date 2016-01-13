namespace Atata
{
    [UIComponent("a", IgnoreNameEndings = "Button,Link")]
    public class Link<TNavigateTo, TOwner> : Clickable<TNavigateTo, TOwner>
        where TNavigateTo : PageObject<TNavigateTo>, new()
        where TOwner : PageObject<TOwner>
    {
    }
}
