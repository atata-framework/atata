#nullable enable

namespace Atata;

/// <summary>
/// Represents the SVG polygon shape control (<c>&lt;polygon&gt;</c>).
/// Default search finds the first occurring <c>&lt;polygon&gt;</c> element.
/// </summary>
/// <typeparam name="TOwner">The type of the owner page object.</typeparam>
[ControlDefinition("*[name()='polygon']", ComponentTypeName = "polygon")]
public class SvgPolygon<TOwner> : Control<TOwner>
    where TOwner : PageObject<TOwner>
{
}
