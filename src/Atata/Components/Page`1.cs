namespace Atata
{
    /// <summary>
    /// Represents the whole HTML page and is the main base class to inherit for the pages.
    /// Uses the <c>&lt;body&gt;</c> tag as a scope.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    /// <seealso cref="PageObject{T}" />
    [PageObjectDefinition("body", ComponentTypeName = "page", IgnoreNameEndings = "Page,PageObject")]
    public abstract class Page<TOwner> : PageObject<TOwner>
        where TOwner : Page<TOwner>
    {
    }
}
