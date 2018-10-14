namespace Atata
{
    /// <summary>
    /// Represents the &lt;h5&gt; heading tag.
    /// Default search finds the first occurring &lt;h5&gt; element.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("h5", ComponentTypeName = "<h5> heading", IgnoreNameEndings = "Header,Heading")]
    public class H5<TOwner> : Content<string, TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
