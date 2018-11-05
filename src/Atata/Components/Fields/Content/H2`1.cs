namespace Atata
{
    /// <summary>
    /// Represents the <c>h2</c> heading tag.
    /// Default search finds the first occurring <c>h2</c> element.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("h2", ComponentTypeName = "<h2> heading", IgnoreNameEndings = "Header,Heading")]
    public class H2<TOwner> : Content<string, TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
