#nullable enable

namespace Atata;

/// <summary>
/// Indicates that the component should be verified whether it is missing on the specified event.
/// By default occurs upon the page object initialization.
/// </summary>
public class VerifyMissingAttribute : WaitingTriggerAttribute
{
    public VerifyMissingAttribute(TriggerEvents on = TriggerEvents.Init, TriggerPriority priority = TriggerPriority.Medium)
        : base(on, priority)
    {
    }

    protected internal override void Execute<TOwner>(TriggerContext<TOwner> context) =>
        context.Component.Should.WithinSeconds(Timeout, RetryInterval).Not.BePresent();
}
