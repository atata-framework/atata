namespace Atata
{
    /// <summary>
    /// Represents the frameset-based HTML page.
    /// Uses the root <c>&lt;frameset&gt;</c> tag as a scope.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    /// <seealso cref="PageObject{T}" />
    [PageObjectDefinition("frameset", ComponentTypeName = "page", IgnoreNameEndings = "Page,PageObject")]
    public abstract class FrameSetPage<TOwner> : Page<TOwner>
        where TOwner : FrameSetPage<TOwner>
    {
    }
}
