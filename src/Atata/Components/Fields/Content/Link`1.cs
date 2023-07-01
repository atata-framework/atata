namespace Atata;

/// <summary>
/// Represents the link control (<c>&lt;a&gt;</c>).
/// Default search is performed by the content.
/// </summary>
/// <typeparam name="TOwner">The type of the owner page object.</typeparam>
[ControlDefinition("a", ComponentTypeName = "link", IgnoreNameEndings = "Button,Link")]
[FindByContent]
public class Link<TOwner> : Text<TOwner>
    where TOwner : PageObject<TOwner>
{
    /// <summary>
    /// Gets the <see cref="ValueProvider{TValue, TOwner}"/> of the <c>href</c> DOM property.
    /// </summary>
    public ValueProvider<string, TOwner> Href =>
        DomProperties["href"];

    [Obsolete("A typo. Use " + nameof(HrefAttribute) + " instead.")] // Obsolete since v2.8.0.
#pragma warning disable VSSpell001 // Spell Check
    public ValueProvider<string, TOwner> HrefAttribue =>
        HrefAttribute;
#pragma warning restore VSSpell001 // Spell Check

    /// <summary>
    /// Gets the <see cref="ValueProvider{TValue, TOwner}"/> of the <c>href</c> DOM attribute.
    /// </summary>
    public ValueProvider<string, TOwner> HrefAttribute =>
        DomAttributes["href"];
}
