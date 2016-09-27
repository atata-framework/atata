namespace Atata
{
    /// <summary>
    /// Represents the text input control (&lt;input type="text"&gt;). By default is being searched by the label. Handles any input element with type="text" or without the type attribute defined.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("input[@type='text' or not(@type)]")]
    public class TextInput<TOwner> : Input<string, TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
