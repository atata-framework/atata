namespace Atata;

/// <summary>
/// Represents an event that occurs when <see cref="PageObject{TOwner}"/> transition in is completed.
/// That is, navigation to the current page object occurred in the same browser tab
/// by interacting with the previous page object, rather than by directly navigating to a URL.
/// </summary>
public sealed class PageObjectTransitionInCompletedEvent : PageObjectEvent
{
    public PageObjectTransitionInCompletedEvent(UIComponent pageObject)
        : base(pageObject)
    {
    }
}
