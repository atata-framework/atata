namespace Atata
{
    [UIComponent("*", IgnoreNameEndings = "Button,Link")]
    public class ClickableControl<TOwner> : Control<TOwner>
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
