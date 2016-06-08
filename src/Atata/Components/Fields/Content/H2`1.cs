namespace Atata
{
    /// <summary>
    /// Represents the &lt;h2&gt; heading tag.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("h2", ComponentTypeName = "<h2> heading", IgnoreNameEndings = "Header,Heading")]
    public class H2<TOwner> : Text<TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
