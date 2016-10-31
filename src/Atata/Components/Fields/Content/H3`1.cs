namespace Atata
{
    /// <summary>
    /// Represents the &lt;h3&gt; heading tag. Default search finds the first occurring &lt;h3&gt; element.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("h3", ComponentTypeName = "<h3> heading", IgnoreNameEndings = "Header,Heading")]
    [ControlFinding(typeof(FindFirstAttribute))]
    public class H3<TOwner> : Content<string, TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
