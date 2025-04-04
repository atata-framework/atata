namespace Atata;

/// <summary>
/// Specifies the waiting period in seconds.
/// By default occurs after any action.
/// </summary>
public class WaitSecondsAttribute : TriggerAttribute
{
    public WaitSecondsAttribute(double seconds, TriggerEvents on = TriggerEvents.AfterAnyAction, TriggerPriority priority = TriggerPriority.Medium)
        : base(on, priority) =>
        Seconds = seconds;

    public double Seconds { get; }

    protected internal override void Execute<TOwner>(TriggerContext<TOwner> context) =>
        Thread.Sleep((int)(Seconds * 1000));
}
