#nullable enable

namespace Atata;

[Obsolete("Use PageObjectDeInitCompletedEvent instead.")] // Obsolete since v4.0.0.
public sealed class PageObjectDeInitEvent
{
    public PageObjectDeInitEvent(UIComponent pageObject) =>
        PageObject = pageObject;

    /// <summary>
    /// Gets the page object.
    /// </summary>
    public UIComponent PageObject { get; }
}
