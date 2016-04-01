namespace Atata
{
    [UIComponent("input[@type='text' or @type='number' or @type='tel' or not(@type)]")]
    public class NumberInput<TOwner> : Input<decimal?, TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
