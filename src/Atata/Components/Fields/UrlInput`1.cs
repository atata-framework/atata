namespace Atata;

/// <summary>
/// Represents the URL input control (<c>&lt;input type="url"&gt;</c>).
/// Default search is performed by the label.
/// </summary>
/// <typeparam name="TOwner">The type of the owner page object.</typeparam>
[ControlDefinition("input[@type='url']", ComponentTypeName = "URL input")]
public class UrlInput<TOwner> : Input<string, TOwner>
    where TOwner : PageObject<TOwner>
{
}
