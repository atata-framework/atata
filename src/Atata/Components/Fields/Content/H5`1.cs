namespace Atata
{
    /// <summary>
    /// Represents the <c>&lt;h5&gt;</c> heading tag.
    /// Default search finds the first occurring <c>&lt;h5&gt;</c> element.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("h5", ComponentTypeName = "<h5> heading", IgnoreNameEndings = "Header,Heading")]
    public class H5<TOwner> : Content<string, TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
