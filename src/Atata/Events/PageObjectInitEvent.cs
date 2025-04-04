namespace Atata;

[Obsolete("Use PageObjectInitStartedEvent instead.")] // Obsolete since v4.0.0.
public sealed class PageObjectInitEvent : PageObjectEvent
{
    public PageObjectInitEvent(UIComponent pageObject)
        : base(pageObject)
    {
    }
}
