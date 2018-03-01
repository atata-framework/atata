namespace Atata
{
    /// <summary>
    /// Represents the frameset-based HTML page.
    /// Uses the root &lt;frameset&gt; tag as a scope.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    /// <seealso cref="PageObject{T}" />
    [PageObjectDefinition("frameset", ComponentTypeName = "page", IgnoreNameEndings = "Page,PageObject")]
    public abstract class FrameSetPage<TOwner> : PageObject<TOwner>
        where TOwner : FrameSetPage<TOwner>
    {
    }
}
