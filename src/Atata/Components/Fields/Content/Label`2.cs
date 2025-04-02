#nullable enable

namespace Atata;

/// <summary>
/// Represents the label control (<c>&lt;label&gt;</c>).
/// Default search is performed by the content.
/// </summary>
/// <typeparam name="TValue">The type of the content.</typeparam>
/// <typeparam name="TOwner">The type of the owner page object.</typeparam>
[ControlDefinition("label", ComponentTypeName = "label", IgnoreNameEndings = "Label")]
[FindByContent]
public class Label<TValue, TOwner> : Content<TValue, TOwner>
    where TOwner : PageObject<TOwner>
{
    /// <summary>
    /// Gets the <see cref="ValueProvider{TValue, TOwner}"/> of the <c>for</c> DOM attribute.
    /// </summary>
    public ValueProvider<string?, TOwner> For =>
        DomAttributes["for"];
}
