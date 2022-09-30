namespace Atata
{
    /// <summary>
    /// Represents the number input control.
    /// Default search is performed by the label.
    /// Handles any <c>&lt;input&gt;</c> element with <c>type="number"</c>, <c>type="text"</c> or without the defined <c>type</c> attribute.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("input[@type='number' or @type='text' or not(@type)]", ComponentTypeName = "number input")]
    public class NumberInput<TOwner> : Input<decimal?, TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
