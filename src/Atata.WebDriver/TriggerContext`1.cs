namespace Atata.WebDriver;

/// <summary>
/// Represents a context of triggering event.
/// </summary>
/// <typeparam name="TOwner">The type of the owner.</typeparam>
public sealed class TriggerContext<TOwner>
    where TOwner : PageObject<TOwner>, IPageObject<TOwner>
{
    internal TriggerContext(TriggerEvents @event, IUIComponent<TOwner> component)
    {
        Event = @event;
        Component = component;
    }

    /// <summary>
    /// Gets the triggering event.
    /// </summary>
    public TriggerEvents Event { get; }

    /// <summary>
    /// Gets the target component.
    /// </summary>
    public IUIComponent<TOwner> Component { get; }
}
