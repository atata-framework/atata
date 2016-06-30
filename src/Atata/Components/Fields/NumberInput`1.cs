namespace Atata
{
    /// <summary>
    /// Represents the number input control.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("input[@type='text' or @type='number' or @type='tel' or not(@type)]")]
    public class NumberInput<TOwner> : Input<decimal?, TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
