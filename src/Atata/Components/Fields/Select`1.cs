namespace Atata
{
    /// <summary>
    /// Represents the text select control (<c>&lt;select&gt;</c>).
    /// Default search is performed by the label.
    /// Selects an option using text.
    /// Option selection is configured via <see cref="SelectOptionBehaviorAttribute"/>.
    /// Possible selection behavior attributes are:
    /// <see cref="SelectsOptionByTextAttribute"/>, <see cref="SelectsOptionByValueAttribute"/>,
    /// <see cref="SelectsOptionByLabelAttributeAttribute"/> and <see cref="SelectsOptionByAttributeAttribute"/>.
    /// Default option selection is performed by text using <see cref="SelectsOptionByTextAttribute"/>.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    public class Select<TOwner> : Select<string, TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
