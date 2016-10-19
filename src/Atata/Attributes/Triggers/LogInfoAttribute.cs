namespace Atata
{
    /// <summary>
    /// Defines the information message to be logged on the specified event.
    /// </summary>
    public class LogInfoAttribute : TriggerAttribute
    {
        public LogInfoAttribute(string message, TriggerEvents on, TriggerPriority priority = TriggerPriority.Medium)
            : base(on, priority)
        {
            Message = message;
        }

        public string Message { get; private set; }

        public override void Execute<TOwner>(TriggerContext<TOwner> context)
        {
            context.Log.Info(Message);
        }
    }
}
