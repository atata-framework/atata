namespace Atata
{
    /// <summary>
    /// Represents the <c>&lt;h3&gt;</c> heading tag.
    /// Default search finds the first occurring <c>&lt;h3&gt;</c> element.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("h3", ComponentTypeName = "<h3> heading", IgnoreNameEndings = "Header,Heading")]
    public class H3<TOwner> : Content<string, TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
