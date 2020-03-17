namespace Atata
{
    /// <summary>
    /// Represents the SVG path shape control (<c>&lt;path&gt;</c>).
    /// Default search finds the first occurring <c>&lt;path&gt;</c> element.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("*[name()='path']", ComponentTypeName = "path")]
    public class SvgPath<TOwner> : Control<TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
