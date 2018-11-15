namespace Atata
{
    /// <summary>
    /// Represents the text input control (<c>&lt;input type="text"&gt;</c>).
    /// Default search is performed by the label.
    /// Handles any input element with <c>type="text"</c> or without the defined <c>type</c> attribute.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("input[@type='text' or not(@type)]")]
    public class TextInput<TOwner> : Input<string, TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
