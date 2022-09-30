namespace Atata
{
    /// <summary>
    /// Represents the label control (<c>&lt;label&gt;</c>).
    /// Default search is performed by the content.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    public class Label<TOwner> : Label<string, TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
