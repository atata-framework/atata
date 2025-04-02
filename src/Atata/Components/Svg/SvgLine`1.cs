#nullable enable

namespace Atata;

/// <summary>
/// Represents the SVG line shape control (<c>&lt;line&gt;</c>).
/// Default search finds the first occurring <c>&lt;line&gt;</c> element.
/// </summary>
/// <typeparam name="TOwner">The type of the owner page object.</typeparam>
[ControlDefinition("*[name()='line']", ComponentTypeName = "line")]
public class SvgLine<TOwner> : Control<TOwner>
    where TOwner : PageObject<TOwner>
{
}
