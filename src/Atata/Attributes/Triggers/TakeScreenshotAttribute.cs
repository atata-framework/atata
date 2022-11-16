namespace Atata
{
    /// <summary>
    /// Indicates that a screenshot should be captured with an optional title.
    /// </summary>
    public class TakeScreenshotAttribute : TriggerAttribute
    {
        public TakeScreenshotAttribute(TriggerEvents on, TriggerPriority priority = TriggerPriority.Medium)
            : this(null, on, priority)
        {
        }

        public TakeScreenshotAttribute(string title, TriggerEvents on, TriggerPriority priority = TriggerPriority.Medium)
            : base(on, priority) =>
            Title = title;

        /// <summary>
        /// Gets the title of a screenshot.
        /// </summary>
        public string Title { get; }

        protected internal override void Execute<TOwner>(TriggerContext<TOwner> context) =>
            context.Component.Context.TakeScreenshot(Title);
    }
}
