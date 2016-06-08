namespace Atata
{
    /// <summary>
    /// Represents the &lt;h3&gt; heading tag.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("h3", ComponentTypeName = "<h3> heading", IgnoreNameEndings = "Header,Heading")]
    public class H3<TOwner> : Text<TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
