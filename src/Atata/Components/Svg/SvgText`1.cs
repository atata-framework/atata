namespace Atata
{
    /// <summary>
    /// Represents the SVG text control (<c>&lt;text&gt;</c>).
    /// Default search finds the first occurring <c>&lt;text&gt;</c> element.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("*[name()='text']", ComponentTypeName = "text")]
    public class SvgText<TOwner> : Text<TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
