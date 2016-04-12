namespace Atata
{
    public class LogInfoAttribute : TriggerAttribute
    {
        public LogInfoAttribute(string message, TriggerEvents on = TriggerEvents.AfterAnyAction, TriggerPriority priority = TriggerPriority.Medium, TriggerScope appliesTo = TriggerScope.Self)
            : base(on, priority, appliesTo)
        {
            Message = message;
        }

        public string Message { get; private set; }

        public override void Execute(TriggerContext context)
        {
            context.Log.Info(Message);
        }
    }
}
