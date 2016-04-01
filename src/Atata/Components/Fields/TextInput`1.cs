namespace Atata
{
    [UIComponent("input[@type='text' or @type='password' or not(@type)]")]
    public class TextInput<TOwner> : Input<string, TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
