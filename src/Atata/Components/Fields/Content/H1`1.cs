namespace Atata;

/// <summary>
/// Represents the <c>&lt;h1&gt;</c> heading tag.
/// Default search finds the first occurring <c>&lt;h1&gt;</c> element.
/// </summary>
/// <typeparam name="TOwner">The type of the owner page object.</typeparam>
[ControlDefinition("h1", ComponentTypeName = "<h1> heading", IgnoreNameEndings = "Header,Heading")]
public class H1<TOwner> : Content<string, TOwner>
    where TOwner : PageObject<TOwner>
{
}
