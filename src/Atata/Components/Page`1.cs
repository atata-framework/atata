namespace Atata
{
    /// <summary>
    /// Represents the whole HTML page. Uses the body tag as a scope.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    /// <seealso cref="PageObject{T}" />
    [PageObjectDefinition("body", ComponentTypeName = "page", IgnoreNameEndings = "Page,PageObject")]
    public class Page<TOwner> : PageObject<TOwner>
        where TOwner : Page<TOwner>
    {
    }
}
