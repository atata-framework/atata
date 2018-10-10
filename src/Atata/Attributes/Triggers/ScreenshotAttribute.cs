namespace Atata
{
    /// <summary>
    /// Indicates that the screenshot should be captured with an optional title.
    /// By default occurs before the click.
    /// </summary>
    public class ScreenshotAttribute : TriggerAttribute
    {
        public ScreenshotAttribute(string title = null, TriggerEvents on = TriggerEvents.BeforeClick, TriggerPriority priority = TriggerPriority.Medium)
            : base(on, priority)
        {
            Title = title;
        }

        public string Title { get; private set; }

        protected internal override void Execute<TOwner>(TriggerContext<TOwner> context)
        {
            context.Log.Screenshot(Title);
        }
    }
}
