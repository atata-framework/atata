#nullable enable

namespace Atata;

/// <summary>
/// Represents an event that occurs when <see cref="PageObject{TOwner}"/> is started to initialize.
/// </summary>
public sealed class PageObjectInitStartedEvent : PageObjectEvent
{
    public PageObjectInitStartedEvent(UIComponent pageObject)
        : base(pageObject)
    {
    }
}
