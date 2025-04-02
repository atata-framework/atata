#nullable enable

namespace Atata;

/// <summary>
/// Represents the SVG ellipse shape control (<c>&lt;ellipse&gt;</c>).
/// Default search finds the first occurring <c>&lt;ellipse&gt;</c> element.
/// </summary>
/// <typeparam name="TOwner">The type of the owner page object.</typeparam>
[ControlDefinition("*[name()='ellipse']", ComponentTypeName = "ellipse")]
public class SvgEllipse<TOwner> : Control<TOwner>
    where TOwner : PageObject<TOwner>
{
}
