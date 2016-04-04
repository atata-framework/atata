namespace Atata
{
    [UIComponent("*[self::input[@type='button' or @type='submit' or @type='reset'] or self::button]", IgnoreNameEndings = "Button")]
    public class ButtonControl<TOwner> : ClickableControl<TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
