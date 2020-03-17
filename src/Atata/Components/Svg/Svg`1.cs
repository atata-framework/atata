namespace Atata
{
    /// <summary>
    /// Represents the SVG control (<c>&lt;svg&gt;</c>).
    /// Default search finds the first occurring <c>&lt;svg&gt;</c> element.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("*[name()='svg']", ComponentTypeName = "SVG")]
    public class Svg<TOwner> : Control<TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
