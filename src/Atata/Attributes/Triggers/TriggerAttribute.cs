namespace Atata;

/// <summary>
/// Represents the base attribute class for the triggers.
/// An inherited class should implement <see cref="Execute{TOwner}(TriggerContext{TOwner})"/> method.
/// </summary>
public abstract class TriggerAttribute : MulticastAttribute
{
    protected TriggerAttribute(TriggerEvents on, TriggerPriority priority = TriggerPriority.Medium)
    {
        On = on;
        Priority = priority;
    }

    /// <summary>
    /// Gets or sets the trigger events.
    /// </summary>
    public TriggerEvents On { get; set; }

    /// <summary>
    /// Gets or sets the priority.
    /// The default value is <see cref="TriggerPriority.Medium"/>.
    /// </summary>
    public TriggerPriority Priority { get; set; }

    /// <summary>
    /// Executes the specified trigger action.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    /// <param name="context">The trigger context.</param>
    protected internal abstract void Execute<TOwner>(TriggerContext<TOwner> context)
        where TOwner : PageObject<TOwner>, IPageObject<TOwner>;
}
