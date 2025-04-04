namespace Atata;

/// <summary>
/// Represents the text area control (<c>&lt;textarea&gt;</c>).
/// Default search is performed by the label.
/// </summary>
/// <typeparam name="TOwner">The type of the owner page object.</typeparam>
[ControlDefinition("textarea", IgnoreNameEndings = "TextArea", ComponentTypeName = "text area")]
[FindByLabel]
public class TextArea<TOwner> : EditableTextField<string, TOwner>
    where TOwner : PageObject<TOwner>
{
    /// <summary>
    /// Gets the <see cref="ValueProvider{TValue, TOwner}"/> of the <c>placeholder</c> DOM property.
    /// </summary>
    public ValueProvider<string?, TOwner> Placeholder =>
        DomProperties["placeholder"];
}
