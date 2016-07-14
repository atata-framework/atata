namespace Atata
{
    /// <summary>
    /// Represents the &lt;h1&gt; heading tag.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("h1", ComponentTypeName = "<h1> heading", IgnoreNameEndings = "Header,Heading")]
    [ControlFinding(typeof(FindFirstAttribute))]
    public class H1<TOwner> : Text<TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
