namespace Atata
{
    /// <summary>
    /// Represents the button control.
    /// Default search is performed by the content and value (button by content text and input by value attribute).
    /// Handles any input element with <c>type="button"</c>, <c>type="submit"</c>, <c>type="reset"</c> or <c>button</c> element.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("*[self::input[@type='button' or @type='submit' or @type='reset'] or self::button]", ComponentTypeName = "button", IgnoreNameEndings = "Button")]
    [ControlFinding(FindTermBy.ContentOrValue)]
    public class Button<TOwner> : Control<TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
