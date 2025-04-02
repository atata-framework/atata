#nullable enable

namespace Atata;

/// <summary>
/// Represents the image input control (<c>&lt;input type="image"&gt;</c>).
/// Default search is performed by <c>alt</c> attribute using <see cref="FindByAltAttribute"/>.
/// </summary>
/// <typeparam name="TOwner">The type of the owner page object.</typeparam>
[ControlDefinition("input[@type='image']", ComponentTypeName = "image input")]
[FindByAlt]
public class ImageInput<TOwner> : Control<TOwner>
    where TOwner : PageObject<TOwner>
{
    /// <summary>
    /// Gets the <see cref="ValueProvider{TValue, TOwner}"/> of the <c>src</c> DOM property.
    /// </summary>
    public ValueProvider<string?, TOwner> Source =>
        DomProperties["src"];

    /// <summary>
    /// Gets the <see cref="ValueProvider{TValue, TOwner}"/> of the <c>src</c> DOM attribute.
    /// </summary>
    public ValueProvider<string?, TOwner> SourceAttribute =>
        DomAttributes["src"];

    /// <summary>
    /// Gets the <see cref="ValueProvider{TValue, TOwner}"/> of the <c>alt</c> DOM property.
    /// </summary>
    public ValueProvider<string?, TOwner> Alt =>
        DomProperties["alt"];
}
