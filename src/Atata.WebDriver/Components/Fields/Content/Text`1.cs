namespace Atata.WebDriver;

/// <summary>
/// Represents any element containing text content.
/// Default search finds the first occurring element.
/// </summary>
/// <typeparam name="TOwner">The type of the owner page object.</typeparam>
[SuppressMessage("Minor Code Smell", "S4041:Type names should not match namespaces")]
public class Text<TOwner> : Content<string, TOwner>
    where TOwner : PageObject<TOwner>
{
}
