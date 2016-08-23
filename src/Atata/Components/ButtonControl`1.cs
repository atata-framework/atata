namespace Atata
{
    [ControlDefinition("*[self::input[@type='button' or @type='submit' or @type='reset'] or self::button]", ComponentTypeName = "button", IgnoreNameEndings = "Button")]
    [ControlFinding(FindTermBy.ContentOrValue)]
    public class ButtonControl<TOwner> : Control<TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
