#nullable enable

namespace Atata;

/// <summary>
/// Represents an event that occurs when <see cref="PageObject{TOwner}"/> is started to initialize.
/// </summary>
public sealed class PageObjectInitStartedEvent
{
    public PageObjectInitStartedEvent(UIComponent pageObject) =>
        PageObject = pageObject;

    /// <summary>
    /// Gets the page object.
    /// </summary>
    public UIComponent PageObject { get; }
}
