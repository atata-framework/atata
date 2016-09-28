namespace Atata
{
    /// <summary>
    /// Represents the number input control. By default is being searched by the label. Handles any input element with type="number", type="tel", type="text" or without the type attribute defined.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("input[@type='text' or @type='number' or @type='tel' or not(@type)]")]
    public class NumberInput<TOwner> : Input<decimal?, TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
