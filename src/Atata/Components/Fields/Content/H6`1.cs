namespace Atata
{
    /// <summary>
    /// Represents the &lt;h6&gt; heading tag.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("h6", ComponentTypeName = "<h6> heading", IgnoreNameEndings = "Header,Heading")]
    [ControlFinding(typeof(FindFirstAttribute))]
    public class H6<TOwner> : Text<TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
