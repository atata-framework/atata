namespace Atata
{
    /// <summary>
    /// Represents the SVG polyline shape control (<c>&lt;polyline&gt;</c>).
    /// Default search finds the first occurring <c>&lt;polyline&gt;</c> element.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("*[name()='polyline']", ComponentTypeName = "polyline")]
    public class SvgPolyline<TOwner> : Control<TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
