namespace Atata
{
    /// <summary>
    /// Represents the <c>h3</c> heading tag.
    /// Default search finds the first occurring <c>h3</c> element.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("h3", ComponentTypeName = "<h3> heading", IgnoreNameEndings = "Header,Heading")]
    public class H3<TOwner> : Content<string, TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
