#nullable enable

namespace Atata;

/// <summary>
/// Represents an event that occurs when <see cref="PageObject{TOwner}"/> is initialized.
/// </summary>
public sealed class PageObjectInitCompletedEvent : PageObjectEvent
{
    public PageObjectInitCompletedEvent(UIComponent pageObject)
        : base(pageObject)
    {
    }
}
