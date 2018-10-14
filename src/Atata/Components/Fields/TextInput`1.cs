namespace Atata
{
    /// <summary>
    /// Represents the text input control (&lt;input type="text"&gt;).
    /// Default search is performed by the label.
    /// Handles any input element with type="text" or without the defined type attribute.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("input[@type='text' or not(@type)]")]
    public class TextInput<TOwner> : Input<string, TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
