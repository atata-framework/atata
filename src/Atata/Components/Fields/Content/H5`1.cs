namespace Atata
{
    /// <summary>
    /// Represents the <c>h5</c> heading tag.
    /// Default search finds the first occurring <c>h5</c> element.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("h5", ComponentTypeName = "<h5> heading", IgnoreNameEndings = "Header,Heading")]
    public class H5<TOwner> : Content<string, TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
