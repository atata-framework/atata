namespace Atata
{
    [UIComponent("*", IgnoreNameEndings = "Button,Link")]
    public class Clickable<TOwner> : ClickableBase<TOwner, TOwner>
        where TOwner : PageObject<TOwner>
    {
        protected override TOwner GetResult()
        {
            return Owner;
        }
    }
}
