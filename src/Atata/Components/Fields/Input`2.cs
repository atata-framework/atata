namespace Atata
{
    /// <summary>
    /// Represents the input control (<c>&lt;input&gt;</c>).
    /// Default search is performed by the label.
    /// </summary>
    /// <typeparam name="TValue">The type of the control's value.</typeparam>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("input[not(@type) or (@type!='button' and @type!='submit' and @type!='reset')]", ComponentTypeName = "input")]
    [FindByLabel]
    public class Input<TValue, TOwner> : EditableTextField<TValue, TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
