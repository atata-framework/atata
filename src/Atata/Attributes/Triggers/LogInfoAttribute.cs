namespace Atata
{
    public class LogInfoAttribute : TriggerAttribute
    {
        public LogInfoAttribute(string message, TriggerEvent on = TriggerEvent.AfterAnyAction, TriggerPriority priority = TriggerPriority.Medium, TriggerScope appliesTo = TriggerScope.Self)
            : base(on, priority, appliesTo)
        {
            Message = message;
        }

        public string Message { get; private set; }

        public override void Run(TriggerContext context)
        {
            context.Log.Info(Message);
        }
    }
}
