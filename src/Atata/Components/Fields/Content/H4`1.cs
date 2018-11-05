namespace Atata
{
    /// <summary>
    /// Represents the <c>h4</c> heading tag.
    /// Default search finds the first occurring <c>h4</c> element.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("h4", ComponentTypeName = "<h4> heading", IgnoreNameEndings = "Header,Heading")]
    public class H4<TOwner> : Content<string, TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
