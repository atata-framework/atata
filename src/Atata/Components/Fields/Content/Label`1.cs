namespace Atata
{
    /// <summary>
    /// Represents the &lt;label&gt; element. Default search is performed by the content.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("label", ComponentTypeName = "label", IgnoreNameEndings = "Label")]
    [ControlFinding(FindTermBy.Content)]
    public class Label<TOwner> : Content<string, TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
