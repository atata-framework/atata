namespace Atata
{
    /// <summary>
    /// Represents the number input control. Default search is performed by the label. Handles any input element with type="number", type="tel", type="text" or without the defined type attribute.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("input[@type='text' or @type='number' or @type='tel' or not(@type)]")]
    public class NumberInput<TOwner> : Input<decimal?, TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
