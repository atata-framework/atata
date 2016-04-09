namespace Atata
{
    [ControlDefinition("*[self::input[@type='button' or @type='submit' or @type='reset'] or self::button]", ComponentTypeName = "button", IgnoreNameEndings = "Button")]
    public class ButtonControl<TNavigateTo, TOwner> : ClickableControl<TNavigateTo, TOwner>
        where TNavigateTo : PageObject<TNavigateTo>
        where TOwner : PageObject<TOwner>
    {
    }
}
