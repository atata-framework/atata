#nullable enable

namespace Atata;

/// <summary>
/// Specifies the condition of the component to wait for.
/// By default occurs upon the page object initialization.
/// </summary>
public class WaitForAttribute : WaitUntilAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WaitForAttribute"/> class.
    /// </summary>
    /// <param name="until">The waiting condition.</param>
    /// <param name="on">The trigger events.</param>
    /// <param name="priority">The priority.</param>
    public WaitForAttribute(Until until = Until.Visible, TriggerEvents on = TriggerEvents.Init, TriggerPriority priority = TriggerPriority.Medium)
        : base(until, on, priority)
    {
    }

    protected internal override void Execute<TOwner>(TriggerContext<TOwner> context) =>
        context.Component.Wait(Until, WaitOptions);
}
