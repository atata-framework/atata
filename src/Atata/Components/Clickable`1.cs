namespace Atata
{
    [UIComponent("*", IgnoreNameEndings = "Button,Link")]
    public class Clickable<TOwner> : Control<TOwner>
        where TOwner : PageObject<TOwner>
    {
        protected TNavigateTo ClickAndGoTo<TNavigateTo>()
            where TNavigateTo : PageObject<TNavigateTo>
        {
            Click();
            return Owner.GoTo<TNavigateTo>();
        }
    }
}
