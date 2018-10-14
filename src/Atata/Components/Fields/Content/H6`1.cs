namespace Atata
{
    /// <summary>
    /// Represents the &lt;h6&gt; heading tag.
    /// Default search finds the first occurring &lt;h6&gt; element.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("h6", ComponentTypeName = "<h6> heading", IgnoreNameEndings = "Header,Heading")]
    public class H6<TOwner> : Content<string, TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
