namespace Atata
{
    /// <summary>
    /// Represents the telephone number input control (&lt;input type="tel"&gt;).
    /// Default search is performed by the label.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("input[@type='tel']", ComponentTypeName = "telephone input")]
    public class TelInput<TOwner> : Input<string, TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
