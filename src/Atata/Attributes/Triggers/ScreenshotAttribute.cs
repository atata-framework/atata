namespace Atata
{
    public class ScreenshotAttribute : TriggerAttribute
    {
        public ScreenshotAttribute(string title = null, TriggerEvent on = TriggerEvent.BeforeClick, TriggerPriority priority = TriggerPriority.Medium, TriggerScope appliesTo = TriggerScope.Self)
            : base(on, priority, appliesTo)
        {
            Title = title;
        }

        public string Title { get; private set; }

        public override void Run(TriggerContext context)
        {
            context.Log.Screenshot(Title);
        }
    }
}
