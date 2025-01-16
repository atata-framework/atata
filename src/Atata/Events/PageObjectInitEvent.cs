#nullable enable

namespace Atata;

[Obsolete("Use PageObjectInitStartedEvent instead.")] // Obsolete since v4.0.0.
public sealed class PageObjectInitEvent
{
    public PageObjectInitEvent(UIComponent pageObject) =>
        PageObject = pageObject;

    /// <summary>
    /// Gets the page object.
    /// </summary>
    public UIComponent PageObject { get; }
}
