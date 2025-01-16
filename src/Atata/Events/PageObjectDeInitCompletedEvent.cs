#nullable enable

namespace Atata;

/// <summary>
/// Represents an event that occurs when <see cref="PageObject{TOwner}"/> is deinitialized.
/// </summary>
public sealed class PageObjectDeInitCompletedEvent
{
    public PageObjectDeInitCompletedEvent(UIComponent pageObject) =>
        PageObject = pageObject;

    /// <summary>
    /// Gets the page object.
    /// </summary>
    public UIComponent PageObject { get; }
}
