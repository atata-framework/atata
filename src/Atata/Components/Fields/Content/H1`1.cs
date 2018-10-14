namespace Atata
{
    /// <summary>
    /// Represents the &lt;h1&gt; heading tag.
    /// Default search finds the first occurring &lt;h1&gt; element.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("h1", ComponentTypeName = "<h1> heading", IgnoreNameEndings = "Header,Heading")]
    public class H1<TOwner> : Content<string, TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
