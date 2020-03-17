namespace Atata
{
    /// <summary>
    /// Represents the SVG circle shape control (<c>&lt;circle&gt;</c>).
    /// Default search finds the first occurring <c>&lt;circle&gt;</c> element.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("*[name()='circle']", ComponentTypeName = "circle")]
    public class SvgCircle<TOwner> : Control<TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
