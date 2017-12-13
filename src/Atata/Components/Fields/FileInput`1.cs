namespace Atata
{
    /// <summary>
    /// Represents the file input control (&lt;input type="file"&gt;).
    /// Default search is performed by the label.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("input[@type='file']")]
    public class FileInput<TOwner> : Input<string, TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
