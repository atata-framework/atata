namespace Atata
{
    /// <summary>
    /// Represents the text select control (<c>&lt;select&gt;</c>).
    /// Default search is performed by the label.
    /// Selects an option using string.
    /// Option selection is configured via <see cref="SelectOptionBehaviorAttribute"/>.
    /// Possible selection behavior attributes are: <see cref="SelectByTextAttribute"/>, <see cref="SelectByValueAttribute"/>, <see cref="SelectByLabelAttribute"/> and <see cref="SelectByAttribute"/>.
    /// Default option selection is performed by text using <see cref="SelectByTextAttribute"/>.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    public class Select<TOwner> : Select<string, TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
