namespace Atata
{
    [ControlDefinition("*", ComponentTypeName = "control", IgnoreNameEndings = "Button,Link")]
    public class ClickableControl<TOwner> : Control<TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
