namespace Atata
{
    public class TextInput<TOwner> : TextInput<string, TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
