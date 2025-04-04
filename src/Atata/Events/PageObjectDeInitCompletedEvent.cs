namespace Atata;

/// <summary>
/// Represents an event that occurs when <see cref="PageObject{TOwner}"/> is deinitialized.
/// </summary>
public sealed class PageObjectDeInitCompletedEvent : PageObjectEvent
{
    public PageObjectDeInitCompletedEvent(UIComponent pageObject)
        : base(pageObject)
    {
    }
}
