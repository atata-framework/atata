namespace Atata
{
    /// <summary>
    /// Represents the &lt;h5&gt; heading tag.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("h5", ComponentTypeName = "<h5> heading", IgnoreNameEndings = "Header,Heading")]
    public class H5<TOwner> : Text<TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
