namespace Atata
{
    [UIComponent("*[self::input[@type='button' or @type='submit' or @type='reset'] or self:button]", IgnoreNameEndings = "Button")]
    public class Button<TOwner> : Clickable<TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
