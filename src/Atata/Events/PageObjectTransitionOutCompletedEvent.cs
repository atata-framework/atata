namespace Atata;

/// <summary>
/// Represents an event that occurs when <see cref="PageObject{TOwner}"/> transition out is completed.
/// That is, navigation to the next page object occurred in the same browser tab
/// by interacting with the current page object, rather than by directly navigating to a URL.
/// </summary>
public sealed class PageObjectTransitionOutCompletedEvent : PageObjectEvent
{
    public PageObjectTransitionOutCompletedEvent(UIComponent pageObject)
        : base(pageObject)
    {
    }
}
