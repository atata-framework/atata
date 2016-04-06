namespace Atata
{
    [ControlDefinition("*", IgnoreNameEndings = "Button,Link")]
    public class ClickableControl<TNavigateTo, TOwner> : Control<TOwner>
        where TNavigateTo : PageObject<TNavigateTo>
        where TOwner : PageObject<TOwner>
    {
        public new TNavigateTo Click()
        {
            base.Click();

            if (typeof(TOwner) == typeof(TNavigateTo))
                return (TNavigateTo)(object)Owner;
            else
                return Owner.GoTo<TNavigateTo>();
        }
    }
}
