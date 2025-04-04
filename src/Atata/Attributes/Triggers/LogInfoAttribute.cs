namespace Atata;

/// <summary>
/// Defines the information message to be logged on the specified event.
/// </summary>
public class LogInfoAttribute : TriggerAttribute
{
    public LogInfoAttribute(string message, TriggerEvents on, TriggerPriority priority = TriggerPriority.Medium)
        : base(on, priority) =>
        Message = message;

    public string Message { get; }

    protected internal override void Execute<TOwner>(TriggerContext<TOwner> context) =>
        context.Component.Session.Log.Info(Message);
}
