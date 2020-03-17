namespace Atata
{
    /// <summary>
    /// Represents the SVG rectangle shape control (<c>&lt;rect&gt;</c>).
    /// Default search finds the first occurring <c>&lt;rect&gt;</c> element.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("*[name()='rect']", ComponentTypeName = "rectangle")]
    public class SvgRectangle<TOwner> : Control<TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
