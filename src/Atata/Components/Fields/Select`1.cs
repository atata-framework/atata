namespace Atata
{
    /// <summary>
    /// Represents the text select control (&lt;select&gt;).
    /// Default search is performed by the label.
    /// Selects an option using string.
    /// Control property can be marked with <see cref="SelectByValueAttribute"/> or <see cref="SelectByTextAttribute"/>.
    /// Default option selection is performed by text using <see cref="SelectByTextAttribute"/>.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    public class Select<TOwner> : Select<string, TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
