namespace Atata;

[Obsolete("Use PageObjectDeInitCompletedEvent instead.")] // Obsolete since v4.0.0.
public sealed class PageObjectDeInitEvent : PageObjectEvent
{
    public PageObjectDeInitEvent(UIComponent pageObject)
        : base(pageObject)
    {
    }
}
